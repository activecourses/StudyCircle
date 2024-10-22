 
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.Filters
{
    public class ClubMemberFilter : IAsyncActionFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public ClubMemberFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.ActionArguments.TryGetValue("id", out var id))
            {
                if(!context.HttpContext.User.IsInRole($"Member#{id}"))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }

            await next();
        }
    }

}
