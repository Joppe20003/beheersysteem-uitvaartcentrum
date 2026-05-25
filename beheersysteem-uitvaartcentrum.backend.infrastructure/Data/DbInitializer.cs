using beheersysteem_uitvaartcentrum.backend.domain.Enums;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace beheersysteem_uitvaartcentrum.backend.infrastructure.Data
{
    public static class DbInitializer
    {
        public static void Seed(AuthDbContext authContext)
        {
            if (!authContext.Roles.Any())
            {
                foreach (Roles role in Enum.GetValues<Roles>())
                {
                    authContext.Roles.Add(new IdentityRole
                    {
                        Name = role.ToString(),
                        NormalizedName = role.ToString().ToUpper()
                    });
                }
                authContext.SaveChanges();
            }
        }

        public static void Fixture(UserManager<IdentityUser> userManager, AppDbContext appContext)
        {
            var seedUsers = new List<(string Name, string Email, Roles Role)>
            {
                ("Jan Janssen", "jan.janssen@uitvaart.nl", Roles.UitvaartOndernemer),
                ("Eva de Wit", "eva.dewit@uitvaart.nl", Roles.UitvaartOndernemer),
                ("Pieter Post", "pieter.post@uitvaart.nl", Roles.UitvaartOndernemer),
                ("Sander Solo", "sander.solo@uitvaart.nl", Roles.UitvaartOndernemer),
                ("Henk Extern", "henk.extern@gast.nl", Roles.Externe)
            };

            string defaultPassword = "jemoeteenlangwachtwoordhebben";

            foreach (var userData in seedUsers)
            {
                var existingUser = userManager.FindByEmailAsync(userData.Email).GetAwaiter().GetResult();

                if (existingUser == null)
                {
                    var newUser = new IdentityUser
                    {
                        UserName = userData.Name,
                        Email = userData.Email,
                        NormalizedUserName = userData.Name.ToUpper(),
                        NormalizedEmail = userData.Email.ToUpper(),
                        EmailConfirmed = true
                    };

                    var createResult = userManager.CreateAsync(newUser, defaultPassword).GetAwaiter().GetResult();

                    if (createResult.Succeeded)
                    {
                        userManager.AddToRoleAsync(newUser, userData.Role.ToString()).GetAwaiter().GetResult();
                    }
                }
            }

            if (!appContext.Dossiers.Any())
            {
                var u1 = userManager.FindByEmailAsync("jan.janssen@uitvaart.nl").GetAwaiter().GetResult();
                var u2 = userManager.FindByEmailAsync("eva.dewit@uitvaart.nl").GetAwaiter().GetResult();
                var u3 = userManager.FindByEmailAsync("pieter.post@uitvaart.nl").GetAwaiter().GetResult();

                if (u1 != null && u2 != null)
                {
                    var dossier1 = new DossierModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier Zonder Extra's",
                        Description = "Eigendom van Jan, geen genodigden.",
                        UserId = Guid.Parse(u1.Id),
                        DateCreated = DateTime.UtcNow
                    };

                    var dossier2 = new DossierModel
                    {
                        Id = Guid.NewGuid(),
                        Title = "Dossier Met Gebruikers",
                        Description = "Eigendom van Eva, bevat genodigden.",
                        UserId = Guid.Parse(u2.Id),
                        DateCreated = DateTime.UtcNow
                    };

                    appContext.Dossiers.AddRange(dossier1, dossier2);

                    if (u3 != null)
                    {
                        appContext.DossierInvited.Add(new DossierInvitedModel
                        {
                            DossierId = dossier2.Id,
                            UserId = u3.Id
                        });
                    }

                    appContext.SaveChanges();
                }
            }
        }
    }
}