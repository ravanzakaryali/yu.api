namespace Yu.Application.Handlers;

public record GetOrderValueStatisticsQuery(DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<OrderValueStatisticsResponseDto>;

internal class GetOrderValueStatisticsQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetOrderValueStatisticsQuery, OrderValueStatisticsResponseDto>
{
    public async Task<OrderValueStatisticsResponseDto> Handle(GetOrderValueStatisticsQuery request, CancellationToken cancellationToken)
    {
        // Build base query with date filtering
        var baseQuery = dbContext.Orders.AsQueryable();
        
        // Apply date filtering
        if (request.StartDate.HasValue && request.EndDate.HasValue)
        {
            baseQuery = baseQuery.Where(o => o.CreatedDate >= request.StartDate.Value && o.CreatedDate <= request.EndDate.Value);
        }
        else if (request.StartDate.HasValue)
        {
            baseQuery = baseQuery.Where(o => o.CreatedDate >= request.StartDate.Value);
        }
        else if (request.EndDate.HasValue)
        {
            baseQuery = baseQuery.Where(o => o.CreatedDate <= request.EndDate.Value);
        }
        
        var totalValue = await baseQuery.SumAsync(o => o.TotalPrice, cancellationToken);
        
        // For now, we'll assume all orders are online since PaymentType is hardcoded to Online in existing queries
        // You may need to add a PaymentType field to the Order entity to properly track this
        var onlineOrders = await baseQuery.CountAsync(cancellationToken);
        var offlineOrders = 0; // Currently no offline orders since PaymentType is hardcoded to Online

        return new OrderValueStatisticsResponseDto
        {
            TotalValue = totalValue,
            OnlineOrders = onlineOrders,
            OfflineOrders = offlineOrders
        };
    }
}
