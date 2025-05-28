namespace Yu.Application.Handlers;

public record LogoutCommand : IRequest;

internal class LogoutCommandHandler(
    IUnitOfWorkService unitOfWorkService,
    IHttpContextAccessor httpContextAccessor) : IRequestHandler<LogoutCommand>
{
    public async Task Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        string userId = unitOfWorkService.CurrentUserService.UserId ?? throw new UnauthorizedAccessException();

        _ = await unitOfWorkService.UserService.FindById(userId)
            ?? throw new UnauthorizedAccessException("User not found");

        httpContextAccessor.HttpContext?.Response.Cookies.Append("token", "delete", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });
    }
}