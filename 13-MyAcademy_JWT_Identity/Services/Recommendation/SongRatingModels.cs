using Microsoft.ML;
using Microsoft.ML.Trainers;

namespace _13_MyAcademy_JWT_Identity.Services.Recommendation;

/// <summary>ML.NET Matrix Factorization giriþ modeli.</summary>
public sealed class SongRating
{
    public float UserId { get; set; }
    public float SongId { get; set; }
    /// <summary>Kullanýcýnýn þarkýyý kaç kez dinlediði (rating olarak kullanýlýr).</summary>
    public float Label { get; set; }
}

/// <summary>ML.NET tahmin çýktý modeli.</summary>
public sealed class SongRatingPrediction
{
    public float Score { get; set; }
}
