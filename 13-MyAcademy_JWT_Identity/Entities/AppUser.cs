using Microsoft.AspNetCore.Identity;

namespace _13_MyAcademy_JWT_Identity.Entities
{
    public class AppUser: IdentityUser<int>
    {
        public int? PackageId { get; set; }
        public Package? Package { get; set; }
        public ICollection<UserSongHistory> UserSongHistories { get; set; } = new List<UserSongHistory>();
    }
}
