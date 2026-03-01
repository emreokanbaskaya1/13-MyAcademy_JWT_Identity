using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.DTOs;
using _13_MyAcademy_JWT_Identity.DTOs.UserDtos;
using _13_MyAcademy_JWT_Identity.Entities;
using _13_MyAcademy_JWT_Identity.Services.JwtServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(
        UserManager<AppUser> _userManager,
        SignInManager<AppUser> signInManager,
        IJwtService _jwtService,
        AppDbContext _context) : ControllerBase
    {
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PackageId = 6 // Varsayılan: Free paketi
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("Kullanıcı kaydı başarılı");
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest("Kullanıcı adı veya şifre hatalı");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            var token = await _jwtService.GenerateTokenAsync(user);

            return Ok(new { token = token });
        }

        /// <summary>
        /// Giriş yapmış kullanıcının profil bilgilerini getirir.
        /// </summary>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var user = await _context.Users
                .Include(u => u.Package)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            return Ok(new UserProfileDto
            {
                Id = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                PackageName = user.Package?.Name ?? "Free",
                PackageLevel = user.Package?.ContentLevel ?? 6
            });
        }

        /// <summary>
        /// Kullanıcının paketini günceller.
        /// </summary>
        [HttpPut("package")]
        [Authorize]
        public async Task<IActionResult> UpdatePackage([FromBody] UpdatePackageDto model)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı");

            var package = await _context.Packages.FindAsync(model.PackageId);
            if (package == null)
                return BadRequest("Geçersiz paket");

            user.PackageId = model.PackageId;
            await _context.SaveChangesAsync();

            // Yeni token oluştur (güncel paket bilgisi ile)
            var token = await _jwtService.GenerateTokenAsync(user);

            return Ok(new { message = $"Paket '{package.Name}' olarak güncellendi", token = token });
        }
    }
}
