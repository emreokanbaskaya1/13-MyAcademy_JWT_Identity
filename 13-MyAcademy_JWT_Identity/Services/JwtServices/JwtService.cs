using _13_MyAcademy_JWT_Identity.Context;
using _13_MyAcademy_JWT_Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _13_MyAcademy_JWT_Identity.Services.JwtServices
{
    public class JwtService(UserManager<AppUser> _userManager, IConfiguration _configuration, AppDbContext _context) : IJwtService
    {
        public async Task<string> GenerateTokenAsync(AppUser user)
        {
            // Kullanıcının paket bilgisini yükle
            var userWithPackage = await _context.Users
                .Include(u => u.Package)
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            var packageLevel = userWithPackage?.Package?.ContentLevel ?? 6; // Varsayılan: Free (6)

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("PackageLevel", packageLevel.ToString()),
                new Claim("PackageName", userWithPackage?.Package?.Name ?? "Free"),
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach(var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            SymmetricSecurityKey symmetricSecurityKey = new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));


            JwtSecurityToken jwtSecurityToken = new(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireInMinutes"])),
                signingCredentials: new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256)
                
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

            return token;


        }
    }
}

