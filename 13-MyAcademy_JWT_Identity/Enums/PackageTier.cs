namespace _13_MyAcademy_JWT_Identity.Enums
{
    /// <summary>
    /// Package tier hierarchy. Lower numeric value = higher tier.
    /// Comparison: userTier <= songTier → access granted.
    /// </summary>
    public enum PackageTier
    {
        Elite = 1,
        Premium = 2,
        Gold = 3,
        Standard = 4,
        Basic = 5,
        Free = 6
    }

    public static class PackageTierExtensions
    {
        /// <summary>
        /// Checks if a user with this tier can access content of the target tier.
        /// A user can access content at their own level or any lower level.
        /// </summary>
        public static bool CanAccess(this PackageTier userTier, PackageTier contentTier)
            => userTier <= contentTier;

        /// <summary>
        /// Converts an integer ContentLevel to a PackageTier enum.
        /// Defaults to Free if the value is out of range.
        /// </summary>
        public static PackageTier ToPackageTier(this int contentLevel)
            => Enum.IsDefined(typeof(PackageTier), contentLevel)
                ? (PackageTier)contentLevel
                : PackageTier.Free;

        /// <summary>
        /// Gets the display name for a PackageTier.
        /// </summary>
        public static string ToDisplayName(this PackageTier tier)
            => tier.ToString();
    }
}
