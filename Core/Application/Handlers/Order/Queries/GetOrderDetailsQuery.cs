namespace Yu.Application.Handlers;

public record GetOrderDetailsQuery(int OrderId) : IRequest<OrderDetailsResponseDto>;

internal class GetOrderDetailsQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetOrderDetailsQuery, OrderDetailsResponseDto>
{
    public async Task<OrderDetailsResponseDto> Handle(GetOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        Order order = await dbContext.Orders
            .Include(o => o.Services)
            .Include(o => o.OrderStatusHistories)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        OrderDetailsResponseDto orderResponse = new()
        {
            OrderNumber = order.OrderNumber,
            Comment = order.Comment,
            TotalPrice = order.TotalPrice,
            OrderStatusHistory = [.. order.OrderStatusHistories
                .Select(osh => new OrderStatusHistoryResponseDto
                {
                    OrderStatus = osh.OrderStatus,
                    SubStatus = osh.SubStatus
                })],
        };



        return orderResponse;

    }

}