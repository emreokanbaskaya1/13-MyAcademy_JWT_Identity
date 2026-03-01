using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using _13_MyAcademy_JWT_Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserSongHistoriesController(AppDbContext _context) : ControllerBase
    {
        /// <summary>
        /// Kullanıcının dinleme geçmişini listeler.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var history = await _context.UserSongHistories
                .Where(h => h.UserId == userId)
                .Include(h => h.Song)
                    .ThenInclude(s => s.Album)
                        .ThenInclude(a => a.Artist)
                .OrderByDescending(h => h.PlayedAt)
                .Select(h => new UserSongHistoryDto
                {
                    Id = h.Id,
                    SongTitle = h.Song.Title,
                    ArtistName = h.Song.Album.Artist.Name,
                    CoverImageUrl = h.Song.Album.CoverImageUrl,
                    PlayedAt = h.PlayedAt
                })
                .Take(50)
                .ToListAsync();

            return Ok(history);
        }

        /// <summary>
        /// Dinleme geçmişine yeni kayıt ekler.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> RecordHistory([FromBody] CreateHistoryDto model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var song = await _context.Songs.FindAsync(model.SongId);
            if (song == null)
                return NotFound("Şarkı bulunamadı");

            var history = new UserSongHistory
            {
                UserId = userId,
                SongId = model.SongId,
                PlayedAt = DateTime.UtcNow
            };

            _context.UserSongHistories.Add(history);
            await _context.SaveChangesAsync();

            return Ok("Dinleme geçmişi kaydedildi");
        }
    }
}
