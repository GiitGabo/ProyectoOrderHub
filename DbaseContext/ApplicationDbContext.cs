using JarredsOrderHub.Models;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.DbaseContext
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Rol> Roles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
