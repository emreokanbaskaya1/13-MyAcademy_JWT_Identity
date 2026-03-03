using _13_MyAcademy_JWT_Identity.Enums;
using System.Security.Claims;

namespace _13_MyAcademy_JWT_Identity.Services.PackageAccess
{
    /// <summary>
    /// Implements package-based access validation using the PackageTier enum.
    /// No hardcoded if-else chains — comparison relies on enum ordering.
    /// </summary>
    public class PackageAccessService : IPackageAccessService
    {
        /// <inheritdoc />
        public bool CanAccess(int userPackageLevel, int contentLevel)
        {
            var userTier = userPackageLevel.ToPackageTier();
            var contentTier = contentLevel.ToPackageTier();

            return userTier.CanAccess(contentTier);
        }

        /// <inheritdoc />
        public string GetPackageName(int contentLevel)
        {
            return contentLevel.ToPackageTier().ToDisplayName();
        }

        /// <inheritdoc />
        public int GetUserPackageLevelFromClaims(ClaimsPrincipal user)
        {
            var packageLevelClaim = user.FindFirst("PackageLevel");

            if (packageLevelClaim != null && int.TryParse(packageLevelClaim.Value, out int level))
            {
                return level;
            }

            // Default: Free tier (guests / users without a package claim)
            return (int)PackageTier.Free;
        }
    }
}
