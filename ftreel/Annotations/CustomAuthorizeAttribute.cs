namespace ftreel.Annotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

public class CustomAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // If user is authenticated.
        if (context.HttpContext.User.Identity is { IsAuthenticated: false })
        {
            // 401 Status code.
            context.Result = new UnauthorizedResult();
            return;
        }

        // If user has role.
        if (!string.IsNullOrEmpty(Roles) && !context.HttpContext.User.IsInRole(Roles))
        {
            // 403 status code.
            context.Result = new ForbidResult();
        }
    }
}