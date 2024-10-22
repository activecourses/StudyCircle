
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Filters
{
    public class ClubOwnerFilter : IAsyncActionFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public ClubOwnerFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        { 
            if (context.ActionArguments.TryGetValue("id", out var id))
            {
                if (!context.HttpContext.User.IsInRole($"Owner#{id}"))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            await next();
        }
    }

}
