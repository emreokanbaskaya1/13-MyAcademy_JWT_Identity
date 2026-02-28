using _13_MyAcademy_JWT_Identity.DTOs.UserDtos;
using _13_MyAcademy_JWT_Identity.Entities;
using _13_MyAcademy_JWT_Identity.Services.JwtServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace _13_MyAcademy_JWT_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(UserManager<AppUser> _userManager, SignInManager<AppUser> signInManager, IJwtService _jwtService) : ControllerBase
    {
        
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok("User registration successfull");
        }

        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (!result.Succeeded)
            {
                return BadRequest("Username or password is not correct");
            }

            var user = await _userManager.FindByNameAsync(model.UserName);

            var token = await _jwtService.GenerateTokenAsync(user);

            return Ok(new { token = token });
        }

    }
}
