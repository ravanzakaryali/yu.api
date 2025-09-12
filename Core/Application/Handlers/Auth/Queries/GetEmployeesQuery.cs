namespace Yu.Application.Handlers;

public record GetEmployeesQuery(GetEmployeesFilterRequestDto? Filter = null) : IRequest<PaginatedResponseDto<EmployeeResponseDto>>;

internal class GetEmployeesQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetEmployeesQuery, PaginatedResponseDto<EmployeeResponseDto>>
{
    public async Task<PaginatedResponseDto<EmployeeResponseDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Users
            .Where(u => !u.IsDeleted)
            .Where(u => !(u is Member))
            .AsQueryable();

        // Filter tətbiq et
        if (request.Filter != null)
        {
            // Search filter (ad, username və ya email)
            if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
            {
                var searchTerm = request.Filter.SearchTerm.ToLower();
                query = query.Where(u => 
                    u.FullName.ToLower().Contains(searchTerm) ||
                    (u.UserName != null && u.UserName.ToLower().Contains(searchTerm)) ||
                    (u.Email != null && u.Email.ToLower().Contains(searchTerm)));
            }

            // Yaradılma tarixi filter
            if (request.Filter.CreatedDateFrom.HasValue)
            {
                query = query.Where(u => u.CreatedDate >= request.Filter.CreatedDateFrom.Value);
            }

            if (request.Filter.CreatedDateTo.HasValue)
            {
                query = query.Where(u => u.CreatedDate <= request.Filter.CreatedDateTo.Value);
            }

            // Aktiv status filter
            if (request.Filter.IsActive.HasValue)
            {
                query = query.Where(u => u.IsActive == request.Filter.IsActive.Value);
            }
        }

        // Ümumi sayı hesabla
        var totalCount = await query.CountAsync(cancellationToken);

        // Pagination parametrlərini təyin et
        var pageNumber = request.Filter?.PageNumber ?? 1;
        var pageSize = request.Filter?.PageSize ?? 10;
        
        // Səhifələmə tətbiq et
        var employees = await query
            .OrderByDescending(u => u.CreatedDate) // Ən yeni employeelər əvvəldə
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
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

        // Pagination response yarat
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        
        return new PaginatedResponseDto<EmployeeResponseDto>
        {
            Items = employees,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }
} 