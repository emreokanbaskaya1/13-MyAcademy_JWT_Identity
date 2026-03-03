namespace _13_MyAcademy_JWT_Identity.DTOs
{
    /// <summary>
    /// Standard error response returned when a user tries to access
    /// content above their package tier.
    /// Frontend uses the "error" field to trigger SweetAlert.
    /// </summary>
    public class PackageUpgradeRequiredResponse
    {
        public string Error { get; init; } = "PACKAGE_UPGRADE_REQUIRED";
        public string Message { get; init; } = "Please upgrade your package";
        public string CurrentPackage { get; init; } = string.Empty;
        public string RequiredPackage { get; init; } = string.Empty;
    }
}
