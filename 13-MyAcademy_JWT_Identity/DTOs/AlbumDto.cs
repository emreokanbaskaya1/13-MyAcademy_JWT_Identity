namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class AlbumDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public int ReleaseYear { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public int SongCount { get; set; }
    }

    public class AlbumDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public int ReleaseYear { get; set; }
        public string ArtistName { get; set; } = string.Empty;
        public List<SongDto> Songs { get; set; } = new();
    }
}
