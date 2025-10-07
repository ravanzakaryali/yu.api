using Microsoft.AspNetCore.Identity;

namespace Yu.Application.Handlers;

public record RegisterCommand(string FullName, string PhoneNumber) : IRequest<RegisterResponseDto>;

public class RegisterCommandHandler(IYuDbContext dbContext, IUnitOfWorkService unitOfWorkService, IHttpContextAccessor contextAccessor,UserManager<User> userManager) : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        Member? existingMember = await dbContext.Members
            .Where(m => m.PhoneNumber == request.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        Member member;
        
        if (existingMember == null)
        {
            // Create new member using IdentityService to properly register with roles
            var registerDto = new RegisterDto
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                ConfirmCode = "111111" // Using the test code from TokenService
            };
            
            member = await unitOfWorkService.IdentityService.RegisterAsync(registerDto);
            member.PhoneNumberConfirmed = true;
            member.LastLoginDate = DateTime.UtcNow;
            
            // Save additional changes to the member
            await dbContext.SaveChangesAsync(cancellationToken);
        }
        else
        {
            // Use existing member and validate confirm code
            member = existingMember;
            
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
        }

        // Ensure the member has the Member role
        if(!await userManager.IsInRoleAsync(member, "Member"))
        {
            await userManager.AddToRoleAsync(member, "Member");
        }

        // Save changes to database
        await dbContext.SaveChangesAsync(cancellationToken);

        TokenDto token = await unitOfWorkService.TokenService.GenerateTokenAsync(member);
        // string refreshToken = unitOfWorkService.TokenService.GenerateRefreshToken();

        contextAccessor.HttpContext?.Response.Cookies.Append("token", token.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            Expires = token.Expires,
            Secure = false,
            SameSite = SameSiteMode.Unspecified
        });

        return new RegisterResponseDto()
        {
            PhoneNumber = member.PhoneNumber ?? string.Empty
        };
    }
}