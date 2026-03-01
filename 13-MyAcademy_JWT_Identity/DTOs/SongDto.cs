namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DurationInSeconds { get; set; }
        public int ContentLevel { get; set; }
        public string AlbumTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
    }
}
