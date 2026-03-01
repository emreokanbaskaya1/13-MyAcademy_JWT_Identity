using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
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

            return Ok(albums);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var album = await _context.Albums
                .Include(a => a.Artist)
                .Include(a => a.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (album == null)
                return NotFound("Albüm bulunamadı");

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

            return Ok(dto);
        }
    }
}
