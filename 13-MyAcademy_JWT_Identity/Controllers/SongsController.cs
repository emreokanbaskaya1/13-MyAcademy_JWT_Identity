using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController(AppDbContext _context) : ControllerBase
    {
        /// <summary>
        /// Tüm şarkıları listeler. Giriş yapılmışsa paket seviyesine göre filtreler.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userPackageLevel = GetUserPackageLevel();

            var songs = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .Where(s => s.ContentLevel >= userPackageLevel)
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

            return Ok(songs);
        }

        /// <summary>
        /// Belirli bir şarkının detayını getirir. Paket kontrolü yapar.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
                return NotFound("Şarkı bulunamadı");

            var userPackageLevel = GetUserPackageLevel();
            if (song.ContentLevel < userPackageLevel)
                return StatusCode(403, "Bu şarkıya erişmek için daha yüksek bir paket gereklidir.");

            return Ok(new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                DurationInSeconds = song.DurationInSeconds,
                ContentLevel = song.ContentLevel,
                AlbumTitle = song.Album.Title,
                ArtistName = song.Album.Artist.Name,
                CoverImageUrl = song.Album.CoverImageUrl
            });
        }

        /// <summary>
        /// MP3 dosyasını stream eder. Yetkilendirme gerektirir.
        /// </summary>
        [HttpGet("stream/{id}")]
        [Authorize]
        public async Task<IActionResult> StreamSong(int id)
        {
            var song = await _context.Songs.FindAsync(id);
            if (song == null)
                return NotFound("Şarkı bulunamadı");

            var userPackageLevel = GetUserPackageLevel();
            if (song.ContentLevel < userPackageLevel)
                return StatusCode(403, "Bu şarkıya erişmek için daha yüksek bir paket gereklidir.");

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", song.FilePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound("Şarkı dosyası bulunamadı");

            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, "audio/mpeg", enableRangeProcessing: true);
        }

        private int GetUserPackageLevel()
        {
            var packageLevelClaim = User.FindFirst("PackageLevel");
            if (packageLevelClaim != null && int.TryParse(packageLevelClaim.Value, out int level))
            {
                return level;
            }
            return 6; // Varsayılan: Free seviyesi
        }
    }
}
