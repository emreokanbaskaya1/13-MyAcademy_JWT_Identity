using Microsoft.AspNetCore.Authorization;

namespace _13_MyAcademy_JWT_Identity.Authorization
{
    public class PackageRequirement : IAuthorizationRequirement
    {
        public int RequiredContentLevel { get; }

        public PackageRequirement(int requiredContentLevel)
        {
            RequiredContentLevel = requiredContentLevel;
        }
    }
}
