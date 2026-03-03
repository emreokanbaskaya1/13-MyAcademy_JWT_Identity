namespace _13_MyAcademy_JWT_Identity.Entities
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int ContentLevel { get; set; } // 1=Elite (en yüksek), 6=Free (en düşük)
        public ICollection<AppUser> Users { get; set; } = new List<AppUser>();
        public ICollection<Song> Songs { get; set; } = new List<Song>();
    }
}
