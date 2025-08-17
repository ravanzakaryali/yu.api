namespace Yu.Application.Handlers;

public record LoginAdminCommand(string Username, string Password) : IRequest<LoginAdminResponseDto>;

internal class LoginAdminCommandHandler(
    IYuDbContext dbContext,
    IUnitOfWorkService unitOfWorkService,
    IHttpContextAccessor contextAccessor) : IRequestHandler<LoginAdminCommand, LoginAdminResponseDto>
{
    public async Task<LoginAdminResponseDto> Handle(LoginAdminCommand request, CancellationToken cancellationToken)
    {
        User? user = await dbContext.Users
                       .Where(m => m.UserName == request.Username)
                       .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                       ?? throw new NotFoundException("User", request.Username);

        await unitOfWorkService.UserService.CheckPasswordAsync(user, request.Password);

        TokenDto token = await unitOfWorkService.TokenService.GenerateTokenAsync(user);

        contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = token.Expires,
            Secure = false,
            SameSite = SameSiteMode.Unspecified
        });

        return new LoginAdminResponseDto()
        {
            Username = user.UserName!,
            FullName = user.FullName,
        };
    }
}