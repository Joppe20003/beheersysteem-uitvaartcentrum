using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using Microsoft.AspNetCore.Authorization;

namespace beheersysteem_uitvaartcentrum.backend.application.Security
{
    public class OverViewDossierAuthorizationHandler : AuthorizationHandler<OverviewDossierAccessRequirement, OverviewDossierDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, OverviewDossierAccessRequirement requirement, OverviewDossierDTO resource)
        {
            string? userId = context.User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;

            if (context.User.IsInRole("Admin"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }
            
            if (userId == resource.UserId.ToString())
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            return Task.CompletedTask;
        }
    }
}
