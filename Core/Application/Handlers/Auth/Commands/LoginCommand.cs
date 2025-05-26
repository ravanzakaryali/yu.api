namespace Yu.Application.Handlers;

public record LoginCommand(string PhoneNumber) : IRequest<LoginResponseDto>;

internal class LoginCommandHandler(IYuDbContext dbContext, IUnitOfWorkService unitOfWorkService) : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        User? user = await dbContext.Users
                    .Where(m => m.PhoneNumber == request.PhoneNumber)
                    .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (user == null)
        {
            return new LoginResponseDto()
            {
                IsRegistered = false,
                PhoneNumber = request.PhoneNumber
            };
        }

        if (user.CreatedConfirmCodeDate.HasValue && DateTime.UtcNow > user.CreatedConfirmCodeDate.Value.AddHours(4))
        {
            user.ConfirmCodeCount = 0;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        int second = user.ConfirmCodeCount * 5 * 60;

        if (user.ConfirmCodeCount == 1)
        {
            second = 1 * 60;
        }

        if (user.ConfirmCodeCount == 6)
        {
            second = 60 * 60;
        }

        if (user.CreatedConfirmCodeDate.HasValue && DateTime.UtcNow < user.CreatedConfirmCodeDate.Value.AddSeconds(second))
        {
            throw new AlreadyExistsException($"Zəhmət olmasa {second / 60} dəqiqə gözləyin. Təsdiqləmə kodu hələ də etibarlıdır.");
        }

        string randomNumber = unitOfWorkService.TokenService.GenerateVerificationCode(6);

        // send confirmation code to user number

        user.ConfirmCode = randomNumber;
        user.ConfirmCodeCount++;
        user.CreatedConfirmCodeDate = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return new LoginResponseDto()
        {
            IsRegistered = true,
            Count = user.ConfirmCodeCount,
            PhoneNumber = user.PhoneNumber!,
        };
    }
}