
namespace Yu.Domain.Entities;

public record CancelOrderCommand(int OrderId, int ReasonId) : IRequest;

internal class CancelOrderCommandHandler(IYuDbContext dbContext) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        order.CancelOrders.Add(new CancelOrder()
        {
            ReasonId = request.ReasonId,
            OrderId = order.Id,
        });
        
        order.IsDeleted = true;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}