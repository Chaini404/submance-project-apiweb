using Microsoft.EntityFrameworkCore;

namespace SubmanceProject.Web.Data
{
    public class SubmanceContext : DbContext
    {
        public SubmanceContext(DbContextOptions<SubmanceContext> options) : base(options) { }

        // Usamos la ruta completa para evitar la ambigüedad con los Models de la Web
        public DbSet<Submance.Domain.Entities.Artista> Artistas { get; set; } = default!;
        public DbSet<Submance.Domain.Entities.Demo> Demos { get; set; } = default!;
        public DbSet<Submance.Domain.Entities.Usuario> Usuarios { get; set; } = default!;
        public DbSet<Submance.Domain.Entities.Genero> Generos { get; set; } = default!;
    }
}