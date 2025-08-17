namespace Yu.API.Middlewares;

public class TokenAutheticationMiddlewares
{
    private readonly RequestDelegate _next;

    public TokenAutheticationMiddlewares(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {

        if (httpContext.Request.Cookies.TryGetValue("token", out string? token))
        {
            httpContext.Request.Headers.Authorization = "Bearer " + token;
            httpContext.Response.Headers.Authorization = "Bearer " + token;
        }

        await _next(httpContext);
    }

}

public class ChangeTokenAutheticationMiddlewares
{
    private readonly RequestDelegate _next;


    public ChangeTokenAutheticationMiddlewares(RequestDelegate next)
    {
        _next = next;
    }
    public async Task InvokeAsync(HttpContext httpContext, IUnitOfWorkService unitOfWork)
    {
        if (httpContext.Request.Cookies.TryGetValue("token", out string? token))
        {
            if (token != null)
            {

                ClaimsPrincipal claimsPrincipal = unitOfWork.TokenService.GetPrincipalFromExpiredToken(token);

                string? loginUserId = claimsPrincipal.GetLoginUserId();
               
                string? tokenExpiration = claimsPrincipal.Claims
                    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (!string.IsNullOrEmpty(tokenExpiration))
                {
                    DateTime expiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(tokenExpiration)).UtcDateTime;


                    if (expiration <= DateTime.UtcNow && loginUserId != null)
                    {
                        // if (!int.TryParse(loginUserId, out int result))
                        // {
                        //     httpContext.Response.Cookies.Append("token", "delete", new CookieOptions
                        //     {
                        //         Expires = DateTime.Now.AddDays(-1),
                        //         HttpOnly = false,
                        //         SameSite = SameSiteMode.None,
                        //         Secure = true,
                        //     });
                        //     await _next(httpContext);
                        //     return;
                        // }
                        User? user = await unitOfWork.UserService.FindByIdAsync(loginUserId);
                        IList<string> roles = await unitOfWork.RoleService.GetRolesByUser(user);

                        TokenDto newAccessToken = await unitOfWork.TokenService.GenerateTokenAsync(user);

                        httpContext.Response.Cookies.Append("token", newAccessToken.AccessToken, new CookieOptions
                        {
                            Expires = newAccessToken.Expires.AddDays(7),
                            HttpOnly = false,
                            SameSite = SameSiteMode.Unspecified,
                            Secure = true,
                        });
                        //httpContext.Request.Cookies = new KeyValuePair<string,string>("token", newAccessToken.AccessToken);

                        List<Claim> claims = new()
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Name, user.UserName!),
                            new Claim(ClaimTypes.Email, user.Email!),
                        };

                        httpContext.Request.Headers.Remove("Authorization");
                        httpContext.Response.Headers.Remove("Authorization");

                        httpContext.Request.Headers.Authorization = "Bearer " + token;
                        httpContext.Response.Headers.Authorization = "Bearer " + token;

                        httpContext.User.AddIdentity(new ClaimsIdentity(claims));
                    }
                }
            }
        }

        await _next(httpContext);
    }

}


public static class TokenAutheticationMiddelwareExtensions
{
    public static IApplicationBuilder UseTokenAuthetication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<TokenAutheticationMiddlewares>();
    }

    public static IApplicationBuilder UseChangeTokenAuthetication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ChangeTokenAutheticationMiddlewares>();
    }
}