namespace _13_MyAcademy_JWT_Identity.Entities
{
    public class Song
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public int DurationInSeconds { get; set; }
        public string FilePath { get; set; } = string.Empty; // MP3 dosya yolu
        public int ContentLevel { get; set; } // 1-6 arası (1=en premium, 6=free)

        // Song-Package relationship: each song belongs to one package
        public int PackageId { get; set; }
        public Package Package { get; set; } = null!;

        public int AlbumId { get; set; }
        public Album Album { get; set; } = null!;
        public ICollection<UserSongHistory> UserSongHistories { get; set; } = new List<UserSongHistory>();
    }
}
