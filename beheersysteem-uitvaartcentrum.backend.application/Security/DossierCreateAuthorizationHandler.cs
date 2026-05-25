using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace beheersysteem_uitvaartcentrum.backend.application.Security
{
    public class DossierCreateAuthorizationHandler : AuthorizationHandler<DossierCreateRequirement, CreateDossierDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DossierCreateRequirement requirement, CreateDossierDTO resource)
        {
            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (context.User.IsInRole("UitvaartOndernemer"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
