using _13_MyAcademy_JWT_Identity.Enums;

namespace _13_MyAcademy_JWT_Identity.Services.PackageAccess
{
    /// <summary>
    /// Encapsulates package-based access validation logic.
    /// </summary>
    public interface IPackageAccessService
    {
        /// <summary>
        /// Determines whether a user with the given package level
        /// can access content at the specified content level.
        /// </summary>
        bool CanAccess(int userPackageLevel, int contentLevel);

        /// <summary>
        /// Returns the PackageTier display name for a given content level.
        /// </summary>
        string GetPackageName(int contentLevel);

        /// <summary>
        /// Extracts the user's package level from JWT claims.
        /// Returns Free (6) if no claim is found.
        /// </summary>
        int GetUserPackageLevelFromClaims(System.Security.Claims.ClaimsPrincipal user);
    }
}
