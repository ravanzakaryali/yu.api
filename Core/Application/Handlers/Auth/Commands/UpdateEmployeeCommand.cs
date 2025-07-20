namespace Yu.Application.Handlers;

public record UpdateEmployeeCommand(string Id, string FullName, string? Email) : IRequest<EmployeeResponseDto>;

internal class UpdateEmployeeCommandHandler(IYuDbContext dbContext) : IRequestHandler<UpdateEmployeeCommand, EmployeeResponseDto>
{
    public async Task<EmployeeResponseDto> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Users
            .Where(u => u.Id == request.Id && !(u is Member))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.Id);

        employee.FullName = request.FullName;
        employee.Email = request.Email;

        await dbContext.SaveChangesAsync(cancellationToken);

        return new EmployeeResponseDto
        {
            Id = employee.Id,
            Username = employee.UserName ?? string.Empty,
            FullName = employee.FullName,
            Email = employee.Email,
            CreatedDate = employee.CreatedDate,
            LastLoginDate = employee.LastLoginDate,
            IsActive = employee.IsActive
        };
    }
} 