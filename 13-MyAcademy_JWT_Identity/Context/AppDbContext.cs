using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using _13_MyAcademy_JWT_Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace _13_MyAcademy_JWT_Identity.Context
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Category> Categories { get; set; }
    }
}
