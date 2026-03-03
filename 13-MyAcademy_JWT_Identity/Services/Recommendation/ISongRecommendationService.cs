namespace _13_MyAcademy_JWT_Identity.Services.Recommendation;

/// <summary>
/// ML.NET tabanlý þarký öneri servisi.
/// UserSongHistory verilerini kullanarak Matrix Factorization ile
/// kullanýcýya þarký önerileri üretir.
/// </summary>
public interface ISongRecommendationService
{
    /// <summary>
    /// Modeli mevcut dinleme geçmiþi verileriyle eðitir / yeniden eðitir.
    /// </summary>
    Task TrainModelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Belirtilen kullanýcý için en yüksek puanlý þarký ID'lerini döndürür.
    /// Model henüz eðitilmemiþse popüler þarkýlarý fallback olarak döner.
    /// </summary>
    Task<List<int>> GetRecommendationsAsync(int userId, int count = 10, CancellationToken cancellationToken = default);

    /// <summary>
    /// Model eðitilmiþ mi?
    /// </summary>
    bool IsModelTrained { get; }
}
