namespace Yu.Application.Handlers;

public record GetEmployeesQuery : IRequest<IEnumerable<EmployeeResponseDto>>;

internal class GetEmployeesQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetEmployeesQuery, IEnumerable<EmployeeResponseDto>>
{
    public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await dbContext.Users
            .Where(u => !u.IsDeleted)
            .Where(u => !(u is Member))
            .Select(u => new EmployeeResponseDto
            {
                Id = u.Id,
                Username = u.UserName ?? string.Empty,
                FullName = u.FullName,
                Email = u.Email,
                CreatedDate = u.CreatedDate,
                LastLoginDate = u.LastLoginDate,
                IsActive = u.IsActive
            })
            .ToListAsync(cancellationToken);

        return employees;
    }
} 