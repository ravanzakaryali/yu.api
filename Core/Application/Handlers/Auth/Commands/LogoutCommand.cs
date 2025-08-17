namespace Yu.Application.Handlers;

public record LogoutCommand : IRequest;

internal class LogoutCommandHandler(
    IUnitOfWorkService unitOfWorkService,
    ICurrentUserService currentUserService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        string userId = currentUserService.UserId ?? throw new UnauthorizedAccessException();

        _ = await unitOfWorkService.UserService.FindByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found");

        httpContextAccessor.HttpContext?.Response.Cookies.Append("token", "delete", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
            SameSite = SameSiteMode.Unspecified,
            Secure = true
        });
    }
}