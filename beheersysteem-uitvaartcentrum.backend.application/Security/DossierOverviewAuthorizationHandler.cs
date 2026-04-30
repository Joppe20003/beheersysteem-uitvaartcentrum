using beheersysteem_uitvaartcentrum.backend.application.DTOs.Dossier;
using Microsoft.AspNetCore.Authorization;

namespace beheersysteem_uitvaartcentrum.backend.application.Security
{
    public class DossierOverviewAuthorizationHandler : AuthorizationHandler<DossierOverviewAccessRequirement, OverviewDossierDTO>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DossierOverviewAccessRequirement requirement, OverviewDossierDTO resource)
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

            if (userId != null && resource.InvitedUserIds.Contains(Guid.Parse(userId)))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }


            return Task.CompletedTask;
        }
    }
}
