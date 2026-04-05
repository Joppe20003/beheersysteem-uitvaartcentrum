using beheersysteem_uitvaartcentrum.backend.domain.Models;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(AppDbContext context)
        {
            if (!context.Dossiers.Any())
            {
                context.AddRange(
                    new Dossier
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier 1",
                        Description = "Beschrijving van dossier 1",
                        DateCreated = DateTime.Now
                    },
                    new Dossier
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier 2",
                        Description = "Beschrijving van dossier 2",
                        DateCreated = DateTime.Now
                    },
                    new Dossier
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier 3",
                        Description = "Beschrijving van dossier 3",
                        DateCreated = DateTime.Now
                    }
                );

                context.SaveChanges();
            }
        }
    }
}
