using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    public class HomeController(AppDbContext _context) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Discover()
        {
            var topTracks = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .OrderBy(s => s.ContentLevel)
                .Take(12)
                .Select(s => new SongDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    DurationInSeconds = s.DurationInSeconds,
                    ContentLevel = s.ContentLevel,
                    AlbumTitle = s.Album.Title,
                    ArtistName = s.Album.Artist.Name,
                    CoverImageUrl = s.Album.CoverImageUrl
                })
                .ToListAsync();

            var featuredAlbums = await _context.Albums
                .Include(a => a.Artist)
                .OrderByDescending(a => a.ReleaseYear)
                .Take(8)
                .Select(a => new AlbumDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverImageUrl = a.CoverImageUrl,
                    ReleaseYear = a.ReleaseYear,
                    ArtistName = a.Artist.Name,
                    SongCount = a.Songs.Count
                })
                .ToListAsync();

            var recentlyAdded = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .OrderByDescending(s => s.Id)
                .Take(5)
                .Select(s => new SongDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    DurationInSeconds = s.DurationInSeconds,
                    ContentLevel = s.ContentLevel,
                    AlbumTitle = s.Album.Title,
                    ArtistName = s.Album.Artist.Name,
                    CoverImageUrl = s.Album.CoverImageUrl
                })
                .ToListAsync();

            var model = new DiscoverViewModel
            {
                TopTracks = topTracks,
                FeaturedAlbums = featuredAlbums,
                RecentlyAdded = recentlyAdded
            };

            return View(model);
        }

        public async Task<IActionResult> Artists()
        {
            ViewData["Title"] = "Sanatçılar";
            var artists = await _context.Artists
                .Select(a => new ArtistDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Bio = a.Bio,
                    ImageUrl = a.ImageUrl,
                    AlbumCount = a.Albums.Count
                })
                .ToListAsync();
            return View(artists);
        }

        public async Task<IActionResult> ArtistDetail(int id)
        {
            ViewData["Title"] = "Sanatçı Detay";
            var artist = await _context.Artists
                .Include(a => a.Albums)
                    .ThenInclude(al => al.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null) return NotFound();

            var dto = new ArtistDetailDto
            {
                Id = artist.Id,
                Name = artist.Name,
                Bio = artist.Bio,
                ImageUrl = artist.ImageUrl,
                Albums = artist.Albums.Select(al => new AlbumDto
                {
                    Id = al.Id,
                    Title = al.Title,
                    CoverImageUrl = al.CoverImageUrl,
                    ReleaseYear = al.ReleaseYear,
                    ArtistName = artist.Name,
                    SongCount = al.Songs.Count
                }).ToList()
            };
            return View(dto);
        }

        public async Task<IActionResult> Albums()
        {
            ViewData["Title"] = "Albümler";
            var albums = await _context.Albums
                .Include(a => a.Artist)
                .Select(a => new AlbumDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    CoverImageUrl = a.CoverImageUrl,
                    ReleaseYear = a.ReleaseYear,
                    ArtistName = a.Artist.Name,
                    SongCount = a.Songs.Count
                })
                .ToListAsync();
            return View(albums);
        }

        public async Task<IActionResult> AlbumDetail(int id)
        {
            ViewData["Title"] = "Albüm Detay";
            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null) return NotFound();

            var dto = new AlbumDetailDto
            {
                Id = album.Id,
                Title = album.Title,
                CoverImageUrl = album.CoverImageUrl,
                ReleaseYear = album.ReleaseYear,
                ArtistName = album.Artist.Name,
                Songs = album.Songs.Select(s => new SongDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    DurationInSeconds = s.DurationInSeconds,
                    ContentLevel = s.ContentLevel,
                    AlbumTitle = album.Title,
                    ArtistName = album.Artist.Name,
                    CoverImageUrl = album.CoverImageUrl
                }).ToList()
            };
            return View(dto);
        }

        public async Task<IActionResult> Packages()
        {
            ViewData["Title"] = "Paketler";
            var packages = await _context.Packages
                .OrderBy(p => p.ContentLevel)
                .Select(p => new PackageDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ContentLevel = p.ContentLevel
                })
                .ToListAsync();
            return View(packages);
        }

        public IActionResult SignIn()
        {
            ViewData["Title"] = "Giriş Yap";
            return View();
        }

        public IActionResult SignUp()
        {
            ViewData["Title"] = "Kayıt Ol";
            return View();
        }

        public IActionResult History()
        {
            ViewData["Title"] = "Dinleme Geçmişi";
            return View();
        }
    }
}
