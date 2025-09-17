namespace Yu.Application.Handlers;

public record GetEmployeeDetailQuery(string Id) : IRequest<EmployeeDetailResponseDto>;

internal class GetEmployeeDetailQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetEmployeeDetailQuery, EmployeeDetailResponseDto>
{
    public async Task<EmployeeDetailResponseDto> Handle(GetEmployeeDetailQuery request, CancellationToken cancellationToken)
    {
        var employee = await dbContext.Users
            .Where(u => u.Id == request.Id && !(u is Member))
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException(nameof(User), request.Id);

        return new EmployeeDetailResponseDto
        {
            Id = employee.Id,
            Username = employee.UserName ?? string.Empty,
            FullName = employee.FullName,
            Email = employee.Email,
            CreatedDate = employee.CreatedDate,
            UpdatedDate = employee.UpdatedDate,
            LastLoginDate = employee.LastLoginDate,
        };
    }
}
