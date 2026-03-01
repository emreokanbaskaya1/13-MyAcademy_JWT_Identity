namespace _13_MyAcademy_JWT_Identity.DTOs
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PackageName { get; set; } = "Free";
        public int PackageLevel { get; set; } = 6;
    }

    public class UpdatePackageDto
    {
        public int PackageId { get; set; }
    }
}
