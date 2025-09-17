namespace Yu.Application.Handlers;

public record GetOrderCountStatisticsQuery(DateTime? StartDate = null, DateTime? EndDate = null) : IRequest<OrderCountStatisticsResponseDto>;

internal class GetOrderCountStatisticsQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetOrderCountStatisticsQuery, OrderCountStatisticsResponseDto>
{
    public async Task<OrderCountStatisticsResponseDto> Handle(GetOrderCountStatisticsQuery request, CancellationToken cancellationToken)
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
        
        var totalOrders = await baseQuery.CountAsync(cancellationToken);
        
        // For now, we'll assume delivery vs self-delivery based on order status
        // Orders with "Delivery" status are considered delivery orders
        // Orders with "Completed" status are considered self-delivery orders
        // This is a business logic assumption - you may need to adjust based on your actual business rules
        
        var deliveryOrders = await baseQuery
            .Where(o => o.OrderStatusHistories
                .OrderByDescending(osh => osh.CreatedDate)
                .FirstOrDefault() != null &&
                o.OrderStatusHistories
                    .OrderByDescending(osh => osh.CreatedDate)
                    .First().OrderStatus == OrderStatus.Delivery)
            .CountAsync(cancellationToken);
            
        var selfDeliveryOrders = await baseQuery
            .Where(o => o.OrderStatusHistories
                .OrderByDescending(osh => osh.CreatedDate)
                .FirstOrDefault() != null &&
                o.OrderStatusHistories
                    .OrderByDescending(osh => osh.CreatedDate)
                    .First().OrderStatus == OrderStatus.Completed)
            .CountAsync(cancellationToken);

        return new OrderCountStatisticsResponseDto
        {
            TotalOrders = totalOrders,
            DeliveryOrders = deliveryOrders,
            SelfDeliveryOrders = selfDeliveryOrders
        };
    }
}
