using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using _13_MyAcademy_JWT_Identity.Services.PackageAccess;
using _13_MyAcademy_JWT_Identity.Services.Recommendation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _13_MyAcademy_JWT_Identity.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RecommendationsController(
    ISongRecommendationService _recommendation,
    AppDbContext _context,
    IPackageAccessService _packageAccess) : ControllerBase
{
    /// <summary>
    /// Kullanýcýya ML.NET tabanlý þarký önerileri döndürür.
    /// Model eðitilmemiþse popüler þarkýlarý fallback olarak döner.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetRecommendations([FromQuery] int count = 10)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        var userPackageLevel = _packageAccess.GetUserPackageLevelFromClaims(User);

        var recommendedIds = await _recommendation.GetRecommendationsAsync(userId, count);

        var songs = await _context.Songs
            .Include(s => s.Album)
                .ThenInclude(a => a.Artist)
            .Include(s => s.Package)
            .Where(s => recommendedIds.Contains(s.Id))
            .ToListAsync();

        // Sýralamayý ML skoruna göre koru
        var orderedSongs = recommendedIds
            .Select(id => songs.FirstOrDefault(s => s.Id == id))
            .Where(s => s is not null)
            .Select(s => new RecommendedSongDto
            {
                Id = s!.Id,
                Title = s.Title,
                ArtistName = s.Album.Artist.Name,
                AlbumTitle = s.Album.Title,
                CoverImageUrl = s.Album.CoverImageUrl,
                ArtistImageUrl = s.Album.Artist.ImageUrl,
                DurationInSeconds = s.DurationInSeconds,
                ContentLevel = s.ContentLevel,
                PackageName = s.Package.Name,
                IsAccessible = _packageAccess.CanAccess(userPackageLevel, s.ContentLevel)
            })
            .ToList();

        return Ok(new RecommendationResponseDto
        {
            IsModelTrained = _recommendation.IsModelTrained,
            Songs = orderedSongs
        });
    }

    /// <summary>
    /// Modeli yeniden eðitir. (Admin veya test amaçlý)
    /// </summary>
    [HttpPost("train")]
    public async Task<IActionResult> TrainModel()
    {
        await _recommendation.TrainModelAsync();
        return Ok(new { message = "Model eðitimi tamamlandý", isModelTrained = _recommendation.IsModelTrained });
    }
}
