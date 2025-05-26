
namespace Yu.Application.Handlers;

public record ConfirmCodeCommand(string PhoneNumber, string Code) : IRequest<GetUserResponseDto>;

internal class ConfirmCodeCommandHandler(IYuDbContext dbContext, IHttpContextAccessor contextAccessor, IUnitOfWorkService unitOfWorkService) : IRequestHandler<ConfirmCodeCommand, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(ConfirmCodeCommand request, CancellationToken cancellationToken)
    {
        Member? member = await dbContext.Members
            .Where(x => x.PhoneNumber == request.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
            ?? throw new UnauthorizedAccessException(nameof(Member));

        if (member.CreatedConfirmCodeDate.HasValue && DateTime.UtcNow > member.CreatedConfirmCodeDate.Value.AddHours(4).AddMinutes(15)) throw new UnauthorizedAccessException("Confirm code exp date");

        member.PhoneNumberConfirmed = true;
        member.ConfirmCode = null;
        member.CreatedConfirmCodeDate = null;
        member.LastLoginDate = DateTime.UtcNow;

        TokenDto token = await unitOfWorkService.TokenService.GenerateTokenAsync(member);

        await dbContext.SaveChangesAsync(cancellationToken);

        contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = token.Expires,
            Secure = false,
            SameSite = SameSiteMode.Unspecified
        });

        IList<string> roles = await unitOfWorkService.RoleService.GetRolesByUser(member);

        return new GetUserResponseDto()
        {
            Name = member.Name,
            Surname = member.Surname,
            PhoneNumber = member.PhoneNumber!,
            Roles = roles,
        };
    }
}