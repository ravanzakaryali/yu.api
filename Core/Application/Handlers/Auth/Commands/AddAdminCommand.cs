namespace Yu.Application.Handlers;

public record AddAdminCommand(string FullName, string Username, string Password) : IRequest<LoginAdminResponseDto>;

internal class AddAdminCommandHandler(IUnitOfWorkService unitOfWorkService) : IRequestHandler<AddAdminCommand, LoginAdminResponseDto>
{
    public async Task<LoginAdminResponseDto> Handle(AddAdminCommand request, CancellationToken cancellationToken)
    {
        await unitOfWorkService.UserService.AddAdminAsync(request.FullName, request.Username, request.Password);
        return new LoginAdminResponseDto()
        {
            Username = request.Username,
            FullName = request.FullName,
        };
    }
}