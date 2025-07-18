namespace Yu.Application.Handlers;

public record AddEmployeeCommand(string FullName, string Username, string Password) : IRequest<AddEmployeeResponseDto>;

internal class AddEmployeeCommandHandler(IUnitOfWorkService unitOfWorkService) : IRequestHandler<AddEmployeeCommand, AddEmployeeResponseDto>
{
    public async Task<AddEmployeeResponseDto> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
    {
        await unitOfWorkService.UserService.AddAdminAsync(request.FullName, request.Username, request.Password);
        return new AddEmployeeResponseDto()
        {
            Username = request.Username,
            FullName = request.FullName,
        };
    }
}