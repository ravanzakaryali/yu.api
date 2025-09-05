namespace Yu.Application.Handlers;

public record GetClientsQuery(GetClientsFilterRequestDto? Filter = null) : IRequest<PaginatedResponseDto<ClientResponseDto>>;

public class GetClientsQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetClientsQuery, PaginatedResponseDto<ClientResponseDto>>
{

    public async Task<PaginatedResponseDto<ClientResponseDto>> Handle(GetClientsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Members
            .Include(m => m.Orders)
            .AsQueryable();

        // Filter tətbiq et
        if (request.Filter != null)
        {
            // Search filter (ad, telefon nömrəsi və ya email)
            if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
            {
                var searchTerm = request.Filter.SearchTerm.ToLower();
                query = query.Where(m => 
                    m.FullName.ToLower().Contains(searchTerm) ||
                    (m.PhoneNumber != null && m.PhoneNumber.Contains(searchTerm)) ||
                    (m.Email != null && m.Email.ToLower().Contains(searchTerm)));
            }

            // Qeydiyyat tarixi filter
            if (request.Filter.RegisterDateFrom.HasValue)
            {
                query = query.Where(m => m.CreatedDate >= request.Filter.RegisterDateFrom.Value);
            }

            if (request.Filter.RegisterDateTo.HasValue)
            {
                query = query.Where(m => m.CreatedDate <= request.Filter.RegisterDateTo.Value);
            }
        }

        // Ümumi sayı hesabla
        var totalCount = await query.CountAsync(cancellationToken);

        // Pagination parametrlərini təyin et
        var pageNumber = request.Filter?.PageNumber ?? 1;
        var pageSize = request.Filter?.PageSize ?? 10;
        
        // Səhifələmə tətbiq et
        var clients = await query
            .OrderByDescending(m => m.CreatedDate) // Ən yeni clientlər əvvəldə
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new ClientResponseDto
            {
                Id = m.Id,
                FullName = m.FullName,
                RegisterDate = m.CreatedDate,
                Phone = m.PhoneNumber ?? string.Empty,
                Email = m.Email,
                OrderCount = m.Orders.Count,
                TotalPrice = m.Orders.Sum(o => o.TotalPrice)
            })
            .ToListAsync(cancellationToken);

        // Pagination response yarat
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        
        return new PaginatedResponseDto<ClientResponseDto>
        {
            Items = clients,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }
}