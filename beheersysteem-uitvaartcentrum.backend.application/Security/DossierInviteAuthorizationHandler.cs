using beheersysteem_uitvaartcentrum.backend.domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace beheersysteem_uitvaartcentrum.backend.application.Security
{
    public class DossierInviteAuthorizationHandler : AuthorizationHandler<DossierInviteRequirement, DossierModel>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, DossierInviteRequirement requirement, DossierModel resource)
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
