using Microsoft.EntityFrameworkCore;
using Submance.Domain.Entities;

namespace SubmanceProject.Api.Data
{
    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }

        // ESTA LÍNEA ES VITAL:
        public DbSet<Demo> Demos { get; set; }
    }
}