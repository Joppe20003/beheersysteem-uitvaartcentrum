using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<DossierModel> Dossiers { get; set; } = null!;
        public DbSet<DocumentModel> Documents { get; set; } = null!;
        public DbSet<DossierInvitedModel> DossierInvited { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DossierInvitedModel>()
                .HasKey(d => new { d.DossierId, d.UserId });

            modelBuilder.Entity<DossierInvitedModel>()
                .HasOne(d => d.Dossier)
                .WithMany(d => d.InvitedUsers)
                .HasForeignKey(d => d.DossierId);
        }
    }
}
