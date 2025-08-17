namespace Yu.Application.Handlers;

public record RegisterCommand(string FullName, string PhoneNumber) : IRequest<RegisterResponseDto>;

public class RegisterCommandHandler(IYuDbContext dbContext, IUnitOfWorkService unitOfWorkService, IHttpContextAccessor contextAccessor) : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        Member member = await dbContext.Members
            .Where(m => m.PhoneNumber == request.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Member), request.PhoneNumber);

        TimeSpan expireTimeSpan = member.ConfirmCodeCount switch
        {
            0 => TimeSpan.FromMinutes(1),
            1 => TimeSpan.FromMinutes(3),
            2 => TimeSpan.FromMinutes(10),
            3 => TimeSpan.FromMinutes(30),
            4 => TimeSpan.FromHours(1),
            _ => TimeSpan.FromHours(1)
        };

        Console.WriteLine(DateTime.UtcNow);
        Console.WriteLine(member.ConfirmCodeGeneratedDate);
        if (member.ConfirmCodeGeneratedDate.HasValue && DateTime.UtcNow > member.ConfirmCodeGeneratedDate.Value.Add(expireTimeSpan))
            throw new Exception("Confirm code exp date");



        member.PhoneNumberConfirmed = true;
        member.ConfirmCode = null;
        member.LastLoginDate = DateTime.UtcNow;


        TokenDto token = await unitOfWorkService.TokenService.GenerateTokenAsync(member);
        // string refreshToken = unitOfWorkService.TokenService.GenerateRefreshToken();
        await dbContext.SaveChangesAsync(cancellationToken);

        contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = token.Expires,
            Secure = false,
            SameSite = SameSiteMode.None
        });

        return new RegisterResponseDto()
        {
            PhoneNumber = member.PhoneNumber ?? string.Empty
        };
    }
}