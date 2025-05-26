namespace Yu.Application.Handlers;

public record RegisterCommand(string Name, string Surname, string PhoneNumber) : IRequest<RegisterResponseDto>;

public class RegisterCommandHandler(IYuDbContext dbContext, IUnitOfWorkService unitOfWorkService) : IRequestHandler<RegisterCommand, RegisterResponseDto>
{
    public async Task<RegisterResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        Member? member = await dbContext.Members
            .Where(x => x.PhoneNumber == request.PhoneNumber)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);

        if (member is not null)
            throw new AlreadyExistsException(nameof(Member), request.PhoneNumber);

        string randomConfirmNumber = unitOfWorkService.TokenService.GenerateVerificationCode();

        Member newMember = await unitOfWorkService.IdentityService.RegisterAsync(new RegisterDto
        {
            Name = request.Name,
            Surname = request.Surname,
            PhoneNumber = request.PhoneNumber,
            ConfirmCode = randomConfirmNumber,
        });

        // send confirmation code to user
        await dbContext.SaveChangesAsync(cancellationToken);

        return new RegisterResponseDto
        {
            PhoneNumber = newMember.PhoneNumber!,
        };
    }
}