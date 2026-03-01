namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class DiscoverViewModel
    {
        public List<SongDto> TopTracks { get; set; } = new();
        public List<AlbumDto> FeaturedAlbums { get; set; } = new();
        public List<SongDto> RecentlyAdded { get; set; } = new();
    }
}
