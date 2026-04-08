using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dossier> Dossiers { get; set; } = null!;
        public DbSet<DossierFile> Documents { get; set; } = null!;
    }
}
