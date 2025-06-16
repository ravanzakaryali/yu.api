using Microsoft.AspNetCore.Identity;

namespace Yu.Application.Handlers;

public record UpdateUserCommand(string? Email, string? FullName) : IRequest<GetUserResponseDto>;

internal class UpdateUserCommandHandler(
    IYuDbContext dbContext,
    IUnitOfWorkService unitOfWorkService,
    UserManager<User> userManager) : IRequestHandler<UpdateUserCommand, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        string userId = unitOfWorkService.CurrentUserService.UserId
            ?? throw new UnauthorizedAccessException("User not found");

        User user = await unitOfWorkService.UserService.FindById(userId)
            ?? throw new UnauthorizedAccessException("User not found");

        if (request.FullName is not null)
        {
            user.FullName = request.FullName;
        }

        if (request.Email is not null)
        {
            user.Email = request.Email;
            await userManager.UpdateNormalizedEmailAsync(user);
            //send confirmation code to email
        }
        IList<string> roles = await userManager.GetRolesAsync(user);

        await dbContext.SaveChangesAsync(cancellationToken);

        return new GetUserResponseDto
        {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber!,
            Email = user.Email,
            Roles = roles
        };
    }
}