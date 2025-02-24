using JarredsOrderHub.Models;
using Microsoft.EntityFrameworkCore;

namespace JarredsOrderHub.DbaseContext
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Platillo> Platillos { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Platillo>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Platillos)
                .HasForeignKey(p => p.IdCategoria)
                .IsRequired(false);
        }
    }
}
