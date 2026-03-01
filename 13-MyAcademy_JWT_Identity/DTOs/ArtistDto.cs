namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class ArtistDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public int AlbumCount { get; set; }
    }

    public class ArtistDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public List<AlbumDto> Albums { get; set; } = new();
    }
}
