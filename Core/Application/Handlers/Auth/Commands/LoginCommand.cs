namespace Yu.Application.Handlers;

public record LoginCommand(string PhoneNumber) : IRequest<LoginResponseDto>;

internal class LoginCommandHandler(IYuDbContext dbContext, IUnitOfWorkService unitOfWorkService) : IRequestHandler<LoginCommand, LoginResponseDto>
{
    public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        Member? member = await dbContext.Members.Where(m => m.PhoneNumber == request.PhoneNumber).FirstOrDefaultAsync(cancellationToken: cancellationToken);
        if (member == null)
        {
            return new LoginResponseDto()
            {
                IsRegistered = false,
                PhoneNumber = request.PhoneNumber
            };
        }

        TimeSpan expireTimeSpan = member.ConfirmCodeCount switch
        {
            0 => TimeSpan.FromMinutes(1),
            1 => TimeSpan.FromMinutes(3),
            2 => TimeSpan.FromMinutes(10),
            3 => TimeSpan.FromMinutes(30),
            4 => TimeSpan.FromHours(1),
            _ => TimeSpan.FromHours(1) // 4-dən çox olduqda maksimum müddət
        };

        if (member.ConfirmCodeGeneratedDate.HasValue && DateTime.UtcNow > member.ConfirmCodeGeneratedDate.Value.Add(expireTimeSpan))
        {
            member.ConfirmCodeCount = 0;
            await dbContext.SaveChangesAsync(cancellationToken);
        }

        int remainingSeconds = 0;
        if (member.ConfirmCodeGeneratedDate.HasValue)
        {
            var expireDate = member.ConfirmCodeGeneratedDate.Value.Add(expireTimeSpan);
            var timeUntilExpire = expireDate - DateTime.UtcNow;

            if (timeUntilExpire.TotalSeconds > 0)
                remainingSeconds = (int)timeUntilExpire.TotalSeconds;
        }

        if (remainingSeconds > 0)
            throw new AlreadyExistsException($"Zəhmət olmasa [[{remainingSeconds}]] gözləyin. Təsdiqləmə kodu hələ də etibarlıdır.");

        string randomNumber = unitOfWorkService.TokenService.GenerateVerificationCode(6);
        // int controlId = dbContext.Users.OrderByDescending(u => u.ControlId).FirstOrDefault()?.ControlId ?? 250;
        // await _unitOfWork.SmsService.TrySendSmsMessageAsync(member.PhoneNumber, controlId + 1, randomNumber);
        member.ConfirmCode = "111111";
        // member.ControlId = controlId + 1;
        member.ConfirmCodeCount++;
        member.ConfirmCodeGeneratedDate = DateTime.UtcNow;
        await dbContext.SaveChangesAsync(cancellationToken);
        return new LoginResponseDto()
        {
            IsRegistered = true,
            Count = member.ConfirmCodeCount,
            PhoneNumber = member.PhoneNumber ?? string.Empty,
            TimeSeconds = remainingSeconds
        };
    }
}