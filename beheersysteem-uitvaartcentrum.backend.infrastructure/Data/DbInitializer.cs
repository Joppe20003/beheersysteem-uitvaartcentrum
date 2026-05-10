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
                // Add sample dossiers
            }
        }


        public static void Seed(UserManager<IdentityUser> userManager, AuthDbContext authContext)
        {
            if (!authContext.Roles.Any())
            {
                foreach (Roles role in Enum.GetValues<Roles>())
                {
                    authContext.Roles.Add(new IdentityRole { Name = role.ToString(), NormalizedName = role.ToString().ToUpper() });
                }
            }

            authContext.SaveChanges();
        }
    }
}
