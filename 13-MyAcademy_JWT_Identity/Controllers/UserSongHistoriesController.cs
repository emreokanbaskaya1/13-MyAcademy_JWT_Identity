using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using _13_MyAcademy_JWT_Identity.Entities;
using _13_MyAcademy_JWT_Identity.Services.PackageAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSongHistoriesController(AppDbContext _context, IPackageAccessService _packageAccess) : ControllerBase
    {
        /// <summary>
        /// Kullanıcının dinleme geçmişini listeler.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var baseQuery = _context.UserSongHistories
                .Where(h => h.UserId == userId);

            // Gerçek istatistikler — Take(50) öncesi tüm kayıttan hesapla
            var totalPlays    = await baseQuery.CountAsync();
            var uniqueSongs   = await baseQuery.Select(h => h.SongId).Distinct().CountAsync();
            var uniqueArtists = await baseQuery
                .Join(_context.Songs, h => h.SongId, s => s.Id, (h, s) => s.AlbumId)
                .Join(_context.Albums, albumId => albumId, al => al.Id, (albumId, al) => al.ArtistId)
                .Distinct()
                .CountAsync();

            var items = await baseQuery
                .OrderByDescending(h => h.PlayedAt)
                .Take(50)
                .Select(h => new UserSongHistoryDto
                {
                    Id = h.Id,
                    SongId = h.SongId,
                    SongTitle = h.Song.Title,
                    ArtistName = h.Song.Album.Artist.Name,
                    CoverImageUrl = h.Song.Album.CoverImageUrl,
                    ArtistImageUrl = h.Song.Album.Artist.ImageUrl,
                    PlayedAt = h.PlayedAt
                })
                .ToListAsync();

            return Ok(new HistoryResponseDto
            {
                TotalPlays    = totalPlays,
                UniqueSongs   = uniqueSongs,
                UniqueArtists = uniqueArtists,
                Items         = items
            });
        }

        /// <summary>
        /// Dinleme geçmişine yeni kayıt ekler.
        /// Package access check: user cannot record history for songs above their tier.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RecordHistory([FromBody] CreateHistoryDto model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var song = await _context.Songs
                .Include(s => s.Package)
                .FirstOrDefaultAsync(s => s.Id == model.SongId);

            if (song == null)
                return NotFound(new { error = "NOT_FOUND", message = "Şarkı bulunamadı" });

            // Package access validation
            var userPackageLevel = _packageAccess.GetUserPackageLevelFromClaims(User);

            if (!_packageAccess.CanAccess(userPackageLevel, song.ContentLevel))
            {
                return StatusCode(403, new PackageUpgradeRequiredResponse
                {
                    CurrentPackage = _packageAccess.GetPackageName(userPackageLevel),
                    RequiredPackage = song.Package.Name
                });
            }

            var history = new UserSongHistory
            {
                UserId = userId,
                SongId = model.SongId,
                PlayedAt = DateTime.UtcNow
            };

            _context.UserSongHistories.Add(history);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Dinleme geçmişi kaydedildi" });
        }
    }
}
