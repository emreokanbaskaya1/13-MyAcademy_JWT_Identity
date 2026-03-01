namespace _13_MyAcademy_JWT_Identity.Entities
{
    public class UserSongHistory
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public AppUser User { get; set; } = null!;
        public int SongId { get; set; }
        public Song Song { get; set; } = null!;
        public DateTime PlayedAt { get; set; }
    }
}
