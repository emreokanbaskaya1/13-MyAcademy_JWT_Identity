namespace _13_MyAcademy_JWT_Identity.Entities
{
    public class Album
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public int ReleaseYear { get; set; }
        public int ArtistId { get; set; }
        public Artist Artist { get; set; } = null!;
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
