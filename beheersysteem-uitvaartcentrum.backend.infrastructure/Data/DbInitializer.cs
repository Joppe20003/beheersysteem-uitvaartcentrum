using beheersysteem_uitvaartcentrum.backend.domain.Enums;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Identity;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Fixture(AppDbContext appContext)
        {
            if (!appContext.Dossiers.Any())
            {
                appContext.AddRange(
                    new DossierModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier 1",
                        Description = "Beschrijving van dossier 1",
                        DateCreated = DateTime.UtcNow
                    },
                    new DossierModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier 2",
                        Description = "Beschrijving van dossier 2",
                        DateCreated = DateTime.UtcNow
                    },
                    new DossierModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier 3",
                        Description = "Beschrijving van dossier 3",
                        DateCreated = DateTime.UtcNow
                    }
                );

                appContext.SaveChanges();
            }
        }


        public static void Seed(UserManager<IdentityUser> userManager, AuthDbContext authContext)
        {
            if (!authContext.Roles.Any())
            {
                foreach (Roles role in Enum.GetValues(typeof(Roles)))
                {
                    authContext.Roles.Add(new IdentityRole { Name = role.ToString(), NormalizedName = role.ToString().ToUpper() });
                }
            }

            authContext.SaveChanges();

            if (!userManager.Users.Any())
            {
                IdentityUser adminUser = new IdentityUser { UserName = "admin", Email = "admin@gmail.com" };

                userManager.CreateAsync(adminUser, "hetwachtwoordmoetminimaal16tekensbevatten").GetAwaiter().GetResult();
                userManager.AddToRoleAsync(adminUser, Roles.Admin.ToString()).GetAwaiter().GetResult();
            }
        }
    }
}
