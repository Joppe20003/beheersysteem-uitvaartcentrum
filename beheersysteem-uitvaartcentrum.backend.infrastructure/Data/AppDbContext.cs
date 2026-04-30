using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DossierModel> Dossiers { get; set; } = null!;
        public DbSet<DocumentModel> Documents { get; set; } = null!;
    }
}
