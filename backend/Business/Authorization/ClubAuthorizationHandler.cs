using Database.Models;
using Microsoft.AspNetCore.Authorization;

namespace Business.Authorization
{
    public class ClubAuthorizationHandler : AuthorizationHandler<ClubAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            ClubAuthorizationRequirement requirement)
        {
            var user = context.User;
            var clubId = requirement.ClubId;
            var requiredRole = requirement.RequiredRole;

            if (user.IsInRole($"{requiredRole}#{clubId}"))
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            if (requiredRole == ClubRole.Member)
            {
                if (user.IsInRole($"{ClubRole.Moderator}#{clubId}") ||
                    user.IsInRole($"{ClubRole.Owner}#{clubId}"))
                {
                    context.Succeed(requirement);
                }
            }
            else if (requiredRole == ClubRole.Moderator)
            {
                if (user.IsInRole($"{ClubRole.Owner}#{clubId}"))
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}