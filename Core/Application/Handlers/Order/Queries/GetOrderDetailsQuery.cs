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

        throw new NotFoundException(nameof(Order), request.OrderId);
    }

}