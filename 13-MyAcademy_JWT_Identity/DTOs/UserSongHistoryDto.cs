namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class UserSongHistoryDto
    {
        public int Id { get; set; }
        public int SongId { get; set; }
        public string SongTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string? CoverImageUrl { get; set; }
        public string? ArtistImageUrl { get; set; }
        public DateTime PlayedAt { get; set; }
    }

    public class HistoryResponseDto
    {
        public int TotalPlays { get; set; }
        public int UniqueSongs { get; set; }
        public int UniqueArtists { get; set; }
        public List<UserSongHistoryDto> Items { get; set; } = new();
    }

    public class CreateHistoryDto
    {
        public int SongId { get; set; }
    }
}
