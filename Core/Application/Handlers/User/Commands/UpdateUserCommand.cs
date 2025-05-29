namespace Yu.Application.Handlers;

public record UpdateUserCommand(string? Email, string? PhoneNumber) : IRequest;

internal class UpdateUserCommandHandler(
    IYuDbContext dbContext,
    IUnitOfWorkService unitOfWorkService) : IRequestHandler<UpdateUserCommand>
{
    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        string userId = unitOfWorkService.CurrentUserService.UserId
            ?? throw new UnauthorizedAccessException("User not found");

        User user = await unitOfWorkService.UserService.FindById(userId)
            ?? throw new UnauthorizedAccessException("User not found");

        if (request.Email is not null)
        {
            user.Email = request.Email;
            //send confirmation code to email
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}