
namespace Yu.Application.Handlers;

public record DeleteLoggedUserCommand : IRequest;

internal class DeleteLoggedUserCommandHandler(
    IHttpContextAccessor httpContextAccessor,
    IYuDbContext dbContext,
    IUnitOfWorkService unitOfWorkService,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteLoggedUserCommand>
{
    public async Task Handle(DeleteLoggedUserCommand request, CancellationToken cancellationToken)
    {
        string userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User not found");

        User user = await unitOfWorkService.UserService.FindByIdAsync(userId)
            ?? throw new UnauthorizedAccessException("User not found");

        if (user is not { IsDeleted: false })
            throw new UnauthorizedAccessException("User not found");

        user.IsDeleted = true;

        await dbContext.SaveChangesAsync(cancellationToken);

        httpContextAccessor.HttpContext?.Response.Cookies.Append("token", "delete", new CookieOptions
        {
            Expires = DateTime.Now.AddDays(-1),
            HttpOnly = true,
            SameSite = SameSiteMode.None,
            Secure = true
        });
    }
}