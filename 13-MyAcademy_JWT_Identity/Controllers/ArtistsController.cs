using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
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

            return Ok(artists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var artist = await _context.Artists
                .Include(a => a.Albums)
                    .ThenInclude(al => al.Songs)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (artist == null)
                return NotFound("Sanatçı bulunamadı");

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

            return Ok(dto);
        }
    }
}
