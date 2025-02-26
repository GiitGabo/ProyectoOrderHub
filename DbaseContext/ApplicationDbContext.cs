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
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Platillo> Platillos { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RecuperacionContrasenia> RecuperacionesContrasenias { get; set; }
        public DbSet<Horario> Horarios { get; set; }
        public DbSet<Tareas> Tareas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Platillo>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Platillos)
                .HasForeignKey(p => p.IdCategoria)
                .IsRequired(false);

            modelBuilder.Entity<Horario>().HasData(
                new Horario { IdHorario = 1, HoraInicio = TimeSpan.Parse("08:00:00"), HoraFin = TimeSpan.Parse("17:00:00") },
                new Horario { IdHorario = 2, HoraInicio = TimeSpan.Parse("09:00:00"), HoraFin = TimeSpan.Parse("18:00:00") },
                new Horario { IdHorario = 3, HoraInicio = TimeSpan.Parse("10:00:00"), HoraFin = TimeSpan.Parse("19:00:00") }
            );

            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Administrador", Descripcion = "Acceso total a la pagina.", Permisos = "Administrar usuarios, Ver empleados, Ver tareas, Administrar tareas" }
            );

            modelBuilder.Entity<Tareas>()
            .HasOne(t => t.Empleado)
            .WithMany()
            .HasForeignKey(t => t.IdEmpleado)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
