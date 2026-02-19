using Microsoft.EntityFrameworkCore;
using Submance.Domain.Entities;

namespace SubmanceProject.Web.Data
{
    public class SubmanceContext : DbContext
    {
        public SubmanceContext(DbContextOptions<SubmanceContext> options) : base(options) { }

        public DbSet<Artista> Artistas { get; set; } = default!;
        public DbSet<Usuario> Usuarios { get; set; } = default!;
        public DbSet<Genero> Generos { get; set; } = default!;
        public DbSet<Cancion> Demos { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Forzar relación 1:1 entre Usuario y Artista
            modelBuilder.Entity<Artista>()
                .HasOne(a => a.Usuario)
                .WithOne(u => u.Artista)
                .HasForeignKey<Artista>(a => a.IdUsuario)
                .OnDelete(DeleteBehavior.Cascade);

            // Asegurar que el correo sea único en base de datos
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();
        }
    }
}