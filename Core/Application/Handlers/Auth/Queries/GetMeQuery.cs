namespace Yu.Application.Handlers;

public record GetMeQuery : IRequest<GetUserResponseDto>;

internal class GetMeQueryHandler(
    ICurrentUserService currentUserService,
    IUnitOfWorkService unitOfWorkService,
    IYuDbContext dbContext) : IRequestHandler<GetMeQuery, GetUserResponseDto>
{
    public async Task<GetUserResponseDto> Handle(GetMeQuery request, CancellationToken cancellationToken)
    {
        string? userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("User is not authenticated");

        User? user = await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found");

        IList<string> roles = await unitOfWorkService.RoleService.GetRolesByUser(user);

        return new GetUserResponseDto
        {
            FullName = user.FullName,
            PhoneNumber = user.PhoneNumber!,
            Email = user.Email,
            Roles = roles,
            Address = null
        };
    }
}


