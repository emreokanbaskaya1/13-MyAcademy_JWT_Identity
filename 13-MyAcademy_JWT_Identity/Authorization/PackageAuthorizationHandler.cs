using Microsoft.AspNetCore.Authorization;

namespace _13_MyAcademy_JWT_Identity.Authorization
{
    public class PackageAuthorizationHandler : AuthorizationHandler<PackageRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            PackageRequirement requirement)
        {
            var packageLevelClaim = context.User.FindFirst("PackageLevel");

            if (packageLevelClaim == null)
            {
                return Task.CompletedTask; // Claim yoksa yetki verilmez
            }

            if (int.TryParse(packageLevelClaim.Value, out int userPackageLevel))
            {
                // Kullanıcının paket seviyesi, gerekli seviyeye eşit veya daha düşükse (daha yüksek erişim) erişim ver
                // ContentLevel: 1=Elite (en yüksek), 6=Free (en düşük)
                // Örnek: Elite (1) kullanıcı, ContentLevel 3 içeriğe erişebilir çünkü 1 <= 3
                if (userPackageLevel <= requirement.RequiredContentLevel)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}
