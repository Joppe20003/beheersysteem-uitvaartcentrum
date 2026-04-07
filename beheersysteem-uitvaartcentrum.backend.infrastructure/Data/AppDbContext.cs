using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.EntityFrameworkCore;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Dossier> Dossiers { get; set; } = null!;
        public DbSet<DossierFile> Documents { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Dossier>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Title).IsRequired();
                entity.Property(d => d.Description).IsRequired();
            });

            modelBuilder.Entity<DossierFile>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Title).IsRequired();
                entity.Property(d => d.Extensions).IsRequired();
                entity.HasOne(d => d.Dossier)
                      .WithMany()
                      .HasForeignKey(e => e.Id);
            });
        }
    }
}
