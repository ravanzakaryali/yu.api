
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;

namespace Yu.API.Filters;

public class PermissionEndpointFilter(IYuDbContext dbContext, IHttpContextAccessor contextAccessor, IUnitOfWorkService unitOfWorkService) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        Endpoint? endpoint = context.HttpContext.GetEndpoint();
        if (endpoint == null)
        {
            await next();
            return;
        }

        ControllerActionDescriptor? descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

        // Allow anonymous endpoints to bypass permission checks (use ActionDescriptor metadata for reliability)
        bool allowsAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
        if (allowsAnonymous)
        {
            await next();
            return;
        }

        string? pattern = descriptor?.AttributeRouteInfo?.Template;
        string httpMethod = descriptor?.ActionConstraints?.OfType<HttpMethodActionConstraint>().FirstOrDefault()?.HttpMethods.FirstOrDefault() ?? "GET";

        bool? isAuth = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().Any();

        var authorizeAttributes = context.ActionDescriptor.EndpointMetadata.OfType<AuthorizeAttribute>().ToList();

        List<string> endpointRoles = new();

        if (authorizeAttributes != null)
        {
            foreach (var attr in authorizeAttributes)
            {
                if (!string.IsNullOrEmpty(attr.Roles))
                {
                    var rolesFromEndpoint = attr.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                    endpointRoles.AddRange(rolesFromEndpoint);
                }
            }
        }

        string? userId = contextAccessor.HttpContext?.User.GetLoginUserId();

        if (userId == null)
        {
            if (isAuth is not null and true)
            {
                context.HttpContext.Response.StatusCode = 401;
                await context.HttpContext.Response.WriteAsync("Unauthorized");
                return;
            }
            else
            {
                await next();
                return;
            }
        }

        User? user = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == userId, context.HttpContext.RequestAborted);

        if (user == null)
        {

            context.HttpContext.Response.Cookies.Append("token", "delete", new CookieOptions
            {
                Expires = DateTime.UtcNow.AddDays(-1),
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
            });

            await next();
            return;
        }

        IList<string> roles = await unitOfWorkService.RoleService.GetRolesByUser(user);

        if (!roles.Any())
        {
            context.HttpContext.Response.StatusCode = 403;
            await context.HttpContext.Response.WriteAsync("Forbidden: User has no roles");
            return;
        }

        if (endpointRoles.Count == 0)
        {
            await next();
            return;
        }

        bool hasAccess = roles.Intersect(endpointRoles).Any();
        if (!hasAccess)
        {
            context.HttpContext.Response.StatusCode = 403;
            await context.HttpContext.Response.WriteAsync("Forbidden: User does not have required roles");
            return;
        }

        await next();
    }
}