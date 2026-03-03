using _13_MyAcademy_JWT_Identity.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace _13_MyAcademy_JWT_Identity.Services.Recommendation;

/// <summary>
/// ML.NET Matrix Factorization kullanarak þarký önerisi üreten servis.
/// Singleton olarak kaydedilir; model bellekte tutulur ve periyodik olarak yeniden eðitilebilir.
/// </summary>
public sealed class SongRecommendationService : ISongRecommendationService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SongRecommendationService> _logger;
    private readonly MLContext _mlContext;
    private readonly object _lock = new();

    private ITransformer? _model;
    private PredictionEngine<SongRating, SongRatingPrediction>? _predictionEngine;

    public bool IsModelTrained => _model is not null;

    public SongRecommendationService(
        IServiceScopeFactory scopeFactory,
        ILogger<SongRecommendationService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _mlContext = new MLContext(seed: 42);
    }

    public async Task TrainModelAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("ML.NET model eðitimi baþlýyor...");

        List<SongRating> ratings;

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            // Her (UserId, SongId) çifti için dinleme sayýsýný hesapla
            ratings = await db.UserSongHistories
                .GroupBy(h => new { h.UserId, h.SongId })
                .Select(g => new SongRating
                {
                    UserId = g.Key.UserId,
                    SongId = g.Key.SongId,
                    Label = g.Count()
                })
                .ToListAsync(cancellationToken);
        }

        if (ratings.Count < 4)
        {
            _logger.LogWarning("Yeterli dinleme verisi yok (min 4 satýr gerekli). Model eðitilmedi.");
            return;
        }

        var dataView = _mlContext.Data.LoadFromEnumerable(ratings);

        var options = new MatrixFactorizationTrainer.Options
        {
            MatrixColumnIndexColumnName = nameof(SongRating.UserId),
            MatrixRowIndexColumnName = nameof(SongRating.SongId),
            LabelColumnName = nameof(SongRating.Label),
            NumberOfIterations = 20,
            ApproximationRank = 8,
            LearningRate = 0.1
        };

        var pipeline = _mlContext.Transforms.Conversion
            .MapValueToKey(outputColumnName: nameof(SongRating.UserId), inputColumnName: nameof(SongRating.UserId))
            .Append(_mlContext.Transforms.Conversion
                .MapValueToKey(outputColumnName: nameof(SongRating.SongId), inputColumnName: nameof(SongRating.SongId)))
            .Append(_mlContext.Recommendation().Trainers.MatrixFactorization(options));

        var model = pipeline.Fit(dataView);

        lock (_lock)
        {
            _model = model;
            _predictionEngine?.Dispose();
            _predictionEngine = _mlContext.Model.CreatePredictionEngine<SongRating, SongRatingPrediction>(model);
        }

        _logger.LogInformation("ML.NET model eðitimi tamamlandý. {Count} veri satýrý kullanýldý.", ratings.Count);
    }

    public async Task<List<int>> GetRecommendationsAsync(int userId, int count = 10, CancellationToken cancellationToken = default)
    {
        List<int> allSongIds;
        HashSet<int> listenedSongIds;

        using (var scope = _scopeFactory.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            allSongIds = await db.Songs
                .Select(s => s.Id)
                .ToListAsync(cancellationToken);

            listenedSongIds = (await db.UserSongHistories
                .Where(h => h.UserId == userId)
                .Select(h => h.SongId)
                .Distinct()
                .ToListAsync(cancellationToken))
                .ToHashSet();
        }

        // Model eðitilmemiþse popüler þarkýlarý fallback olarak döndür
        if (!IsModelTrained)
        {
            return await GetPopularFallbackAsync(userId, count, cancellationToken);
        }

        var candidateSongIds = allSongIds
            .Where(id => !listenedSongIds.Contains(id))
            .ToList();

        if (candidateSongIds.Count == 0)
        {
            // Kullanýcý tüm þarkýlarý dinlemiþ — en popülerleri döndür
            return await GetPopularFallbackAsync(userId, count, cancellationToken);
        }

        var scored = new List<(int SongId, float Score)>();

        lock (_lock)
        {
            if (_predictionEngine is null)
                return GetPopularFallbackAsync(userId, count, cancellationToken).GetAwaiter().GetResult();

            foreach (var songId in candidateSongIds)
            {
                var prediction = _predictionEngine.Predict(new SongRating
                {
                    UserId = userId,
                    SongId = songId
                });

                if (!float.IsNaN(prediction.Score))
                {
                    scored.Add((songId, prediction.Score));
                }
            }
        }

        return scored
            .OrderByDescending(x => x.Score)
            .Take(count)
            .Select(x => x.SongId)
            .ToList();
    }

    /// <summary>
    /// Model yoksa veya yeterli veri yoksa kullanýcýnýn dinlemediði en popüler þarkýlarý döndürür.
    /// </summary>
    private async Task<List<int>> GetPopularFallbackAsync(int userId, int count, CancellationToken cancellationToken)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var listenedIds = await db.UserSongHistories
            .Where(h => h.UserId == userId)
            .Select(h => h.SongId)
            .Distinct()
            .ToListAsync(cancellationToken);

        // Platform genelinde en çok dinlenen þarkýlar (kullanýcýnýn dinlemedikleri)
        var popular = await db.UserSongHistories
            .GroupBy(h => h.SongId)
            .OrderByDescending(g => g.Count())
            .Select(g => g.Key)
            .Where(id => !listenedIds.Contains(id))
            .Take(count)
            .ToListAsync(cancellationToken);

        // Eðer hiç popüler yoksa rastgele þarkýlar
        if (popular.Count == 0)
        {
            popular = await db.Songs
                .Where(s => !listenedIds.Contains(s.Id))
                .OrderBy(s => s.Id)
                .Take(count)
                .Select(s => s.Id)
                .ToListAsync(cancellationToken);
        }

        return popular;
    }
}
