namespace _13_MyAcademy_JWT_Identity.DTOs;

public sealed record RecommendedSongDto
{
    public int Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string ArtistName { get; init; } = string.Empty;
    public string AlbumTitle { get; init; } = string.Empty;
    public string? CoverImageUrl { get; init; }
    public string? ArtistImageUrl { get; init; }
    public int DurationInSeconds { get; init; }
    public int ContentLevel { get; init; }
    public string PackageName { get; init; } = string.Empty;
    public bool IsAccessible { get; init; }
    public float Score { get; init; }
}

public sealed record RecommendationResponseDto
{
    public bool IsModelTrained { get; init; }
    public List<RecommendedSongDto> Songs { get; init; } = [];
}
