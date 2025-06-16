namespace Yu.Application.Handlers;

public record GetLoggedUserQuery : IRequest<GetUserResponseDto>;

internal class GetLoggedUserQueryHandler(ICurrentUserService currentUserService, IUnitOfWorkService unitOfWorkService) : IRequestHandler<GetLoggedUserQuery, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(GetLoggedUserQuery request, CancellationToken cancellationToken)
    {
        string? userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        User? user = await unitOfWorkService.UserService.FindById(userId)
            ?? throw new UnauthorizedAccessException("User not found");

        IList<string> roles = await unitOfWorkService.RoleService.GetRolesByUser(user);

        return new GetUserResponseDto
        {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber!,
            Email = user.Email,
            Roles = roles
        };
    }
}