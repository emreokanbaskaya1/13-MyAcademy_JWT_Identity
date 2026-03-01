namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class ProfileViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PackageName { get; set; } = "Free";
        public int PackageLevel { get; set; } = 6;
        public List<SongHistoryDto> RecentHistory { get; set; } = new();
    }

    public class SongHistoryDto
    {
        public string SongTitle { get; set; } = string.Empty;
        public string ArtistName { get; set; } = string.Empty;
        public string AlbumTitle { get; set; } = string.Empty;
        public string? ArtistImageUrl { get; set; }
        public DateTime PlayedAt { get; set; }
    }

    public class ChangePasswordDto
    {
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmNewPassword { get; set; } = string.Empty;
    }
}
