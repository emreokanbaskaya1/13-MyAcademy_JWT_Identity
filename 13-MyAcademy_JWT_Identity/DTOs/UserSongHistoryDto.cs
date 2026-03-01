namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class UserSongHistoryDto
    {
        public int Id { get; set; }
        public string SongTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public DateTime PlayedAt { get; set; }
    }

    public class CreateHistoryDto
    {
        public int SongId { get; set; }
    }
}
