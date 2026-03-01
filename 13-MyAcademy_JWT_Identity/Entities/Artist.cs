namespace _13_MyAcademy_JWT_Identity.Entities
{
    public class Artist
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ImageUrl { get; set; }
        public ICollection<Album> Albums { get; set; } = new List<Album>();
    }
}
