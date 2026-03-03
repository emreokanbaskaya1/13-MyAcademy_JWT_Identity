using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using _13_MyAcademy_JWT_Identity.Services.PackageAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongsController(AppDbContext _context, IPackageAccessService _packageAccess) : ControllerBase
    {
        /// <summary>
        /// Lists all songs. If authenticated, shows songs the user can access
        /// plus songs from higher packages (marked with their package tier).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userPackageLevel = _packageAccess.GetUserPackageLevelFromClaims(User);

            var songs = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .Include(s => s.Package)
                .Select(s => new SongDto
                {
                    Id = s.Id,
                    Title = s.Title,
                    DurationInSeconds = s.DurationInSeconds,
                    ContentLevel = s.ContentLevel,
                    PackageName = s.Package.Name,
                    IsAccessible = _packageAccess.CanAccess(userPackageLevel, s.ContentLevel),
                    AlbumTitle = s.Album.Title,
                    ArtistName = s.Album.Artist.Name,
                    CoverImageUrl = s.Album.CoverImageUrl
                })
                .ToListAsync();

            return Ok(songs);
        }

        /// <summary>
        /// Gets song details by ID. Returns package info for frontend badge display.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .Include(s => s.Package)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
                return NotFound(new { error = "NOT_FOUND", message = "Şarkı bulunamadı" });

            var userPackageLevel = _packageAccess.GetUserPackageLevelFromClaims(User);

            return Ok(new SongDto
            {
                Id = song.Id,
                Title = song.Title,
                DurationInSeconds = song.DurationInSeconds,
                ContentLevel = song.ContentLevel,
                PackageName = song.Package.Name,
                IsAccessible = _packageAccess.CanAccess(userPackageLevel, song.ContentLevel),
                AlbumTitle = song.Album.Title,
                ArtistName = song.Album.Artist.Name,
                CoverImageUrl = song.Album.CoverImageUrl
            });
        }

        /// <summary>
        /// Play Song — validates package access before allowing playback.
        /// Returns:
        ///   200 → Song data + stream URL (access granted)
        ///   401 → Not logged in (handled by [Authorize])
        ///   403 → PACKAGE_UPGRADE_REQUIRED (logged in but insufficient package)
        /// </summary>
        [HttpPost("play/{id}")]
        [Authorize]
        public async Task<IActionResult> PlaySong(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Album)
                    .ThenInclude(a => a.Artist)
                .Include(s => s.Package)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
                return NotFound(new { error = "NOT_FOUND", message = "Şarkı bulunamadı" });

            var userPackageLevel = _packageAccess.GetUserPackageLevelFromClaims(User);

            if (!_packageAccess.CanAccess(userPackageLevel, song.ContentLevel))
            {
                return StatusCode(403, new PackageUpgradeRequiredResponse
                {
                    CurrentPackage = _packageAccess.GetPackageName(userPackageLevel),
                    RequiredPackage = song.Package.Name
                });
            }

            return Ok(new
            {
                song = new SongDto
                {
                    Id = song.Id,
                    Title = song.Title,
                    DurationInSeconds = song.DurationInSeconds,
                    ContentLevel = song.ContentLevel,
                    PackageName = song.Package.Name,
                    IsAccessible = true,
                    AlbumTitle = song.Album.Title,
                    ArtistName = song.Album.Artist.Name,
                    CoverImageUrl = song.Album.CoverImageUrl,
                    ArtistImageUrl = song.Album.Artist.ImageUrl
                },
                streamUrl = $"/api/songs/stream/{song.Id}"
            });
        }

        /// <summary>
        /// Streams the MP3 file. Requires authentication + package access.
        /// Returns:
        ///   200 → Audio stream
        ///   401 → Not logged in
        ///   403 → PACKAGE_UPGRADE_REQUIRED
        /// </summary>
        [HttpGet("stream/{id}")]
        [Authorize]
        public async Task<IActionResult> StreamSong(int id)
        {
            var song = await _context.Songs
                .Include(s => s.Package)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (song == null)
                return NotFound(new { error = "NOT_FOUND", message = "Şarkı dosyası bulunamadı" });

            var userPackageLevel = _packageAccess.GetUserPackageLevelFromClaims(User);

            if (!_packageAccess.CanAccess(userPackageLevel, song.ContentLevel))
            {
                return StatusCode(403, new PackageUpgradeRequiredResponse
                {
                    CurrentPackage = _packageAccess.GetPackageName(userPackageLevel),
                    RequiredPackage = song.Package.Name
                });
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", song.FilePath);

            if (!System.IO.File.Exists(filePath))
                return NotFound(new { error = "NOT_FOUND", message = "Şarkı dosyası bulunamadı" });

            var fileStream = System.IO.File.OpenRead(filePath);
            return File(fileStream, "audio/mpeg", enableRangeProcessing: true);
        }
    }
}
