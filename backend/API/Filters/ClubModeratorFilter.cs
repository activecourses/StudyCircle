
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Filters
{
    public class ClubModeratorFilter : IAsyncActionFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public ClubModeratorFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var id))
            {
                if (!context.HttpContext.User.IsInRole($"Moderator#{id}"))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            await next();
        }
    }

}
