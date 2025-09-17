using Microsoft.AspNetCore.Identity;

namespace Yu.Application.Handlers;

public record EditAccountCommand(string? FullName, string? Email) : IRequest<GetUserResponseDto>;

internal class EditAccountCommandHandler(
    IYuDbContext dbContext,
    IUnitOfWorkService unitOfWorkService,
    ICurrentUserService currentUserService,
    UserManager<User> userManager) : IRequestHandler<EditAccountCommand, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(EditAccountCommand request, CancellationToken cancellationToken)
    {
        string userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User not found");

        User user = await unitOfWorkService.UserService.FindByIdAsync(userId)
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
