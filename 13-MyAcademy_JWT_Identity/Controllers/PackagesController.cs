using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackagesController(AppDbContext _context) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var packages = await _context.Packages
                .Select(p => new PackageDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    ContentLevel = p.ContentLevel
                })
                .OrderBy(p => p.ContentLevel)
                .ToListAsync();

            return Ok(packages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var package = await _context.Packages.FindAsync(id);
            if (package == null)
                return NotFound("Paket bulunamadı");

            return Ok(new PackageDto
            {
                Id = package.Id,
                Name = package.Name,
                ContentLevel = package.ContentLevel
            });
        }
    }
}
