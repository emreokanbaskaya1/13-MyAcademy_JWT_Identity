using _13_MyAcademy_JWT_Identity.Entities;

namespace _13_MyAcademy_JWT_Identity.Services.JwtServices
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(AppUser user);
    }
}
