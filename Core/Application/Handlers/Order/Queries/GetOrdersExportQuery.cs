namespace Yu.Application.Handlers;

public record GetOrdersExportQuery(DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<IEnumerable<OrderExportRowDto>>;

internal class GetOrdersExportQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetOrdersExportQuery, IEnumerable<OrderExportRowDto>>
{
    public async Task<IEnumerable<OrderExportRowDto>> Handle(GetOrdersExportQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Orders
            .Include(o => o.Member)
            .Include(o => o.Address)
            .Include(o => o.OrderStatusHistories)
            .AsQueryable();
        
        // Apply date filtering
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate >= request.StartDate.Value && o.CreatedDate <= request.EndDate.Value);
        }
        else if (request.StartDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate >= request.StartDate.Value);
        }
        else if (request.EndDate.HasValue)
        {
            query = query.Where(o => o.CreatedDate <= request.EndDate.Value);
        }
        
        var orders = await query
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);

        return orders.Select(o =>
        {
            var latestStatus = o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).FirstOrDefault();
            return new OrderExportRowDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,
                CustomerFullName = o.Member.FullName,
                CustomerPhone = o.Member.PhoneNumber ?? string.Empty,
                Address = o.Address.FullAddress,
                TotalPrice = o.TotalPrice,
                OrderStatus = latestStatus != null ? latestStatus.OrderStatus : OrderStatus.PickUp,
                SubStatus = latestStatus != null ? latestStatus.SubStatus : Status.Pending,
            };
        });
    }
}


