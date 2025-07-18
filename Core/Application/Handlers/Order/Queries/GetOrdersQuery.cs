namespace Yu.Application.Handlers;

public record GetOrdersQuery(GetOrdersFilterRequestDto? Filter = null) : IRequest<PaginatedResponseDto<OrderResponseDto>>;

internal class GetOrdersQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetOrdersQuery, PaginatedResponseDto<OrderResponseDto>>
{
    public async Task<PaginatedResponseDto<OrderResponseDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Orders
            .Include(o => o.Member)
            .Include(o => o.PickupDateSetting)
            .Include(o => o.OrderStatusHistories)
            .AsQueryable();

        // Filter tətbiq et
        if (request.Filter != null)
        {
            // Search filter (ad və ya telefon nömrəsi)
            if (!string.IsNullOrWhiteSpace(request.Filter.SearchTerm))
            {
                var searchTerm = request.Filter.SearchTerm.ToLower();
                query = query.Where(o => 
                    o.Member.FullName.ToLower().Contains(searchTerm) ||
                    (o.Member.PhoneNumber != null && o.Member.PhoneNumber.Contains(searchTerm)));
            }

            // Status filter
            if (request.Filter.Status.HasValue)
            {
                query = query.Where(o => o.OrderStatusHistories
                    .OrderByDescending(osh => osh.CreatedDate)
                    .FirstOrDefault() != null &&
                    o.OrderStatusHistories
                        .OrderByDescending(osh => osh.CreatedDate)
                        .First().OrderStatus == request.Filter.Status.Value);
            }

            // Tarix filter
            if (request.Filter.CreatedDateFrom.HasValue)
            {
                query = query.Where(o => o.CreatedDate >= request.Filter.CreatedDateFrom.Value);
            }

            if (request.Filter.CreatedDateTo.HasValue)
            {
                query = query.Where(o => o.CreatedDate <= request.Filter.CreatedDateTo.Value);
            }
        }

        // Ümumi sayı hesabla
        var totalCount = await query.CountAsync(cancellationToken);

        // Pagination parametrlərini təyin et
        var pageNumber = request.Filter?.PageNumber ?? 1;
        var pageSize = request.Filter?.PageSize ?? 10;
        
        // Səhifələmə tətbiq et
        var orders = await query
            .OrderByDescending(o => o.CreatedDate) // Ən yeni orderlər əvvəldə
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,
                Comment = o.Comment,
                Address = o.Address.FullAddress,
                PaymentType = PaymentType.Online,
                TotalPrice = o.TotalPrice,
                User = new UserDto
                {
                    FullName = o.Member.FullName,
                    PhoneNumber = o.Member.PhoneNumber ?? string.Empty
                },
                OrderStatus = o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).FirstOrDefault() != null ?
                    o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).First().OrderStatus : OrderStatus.PickUp,
            })
            .ToListAsync(cancellationToken);

        // Pagination response yarat
        var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
        
        return new PaginatedResponseDto<OrderResponseDto>
        {
            Items = orders,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages,
            HasPreviousPage = pageNumber > 1,
            HasNextPage = pageNumber < totalPages
        };
    }
}