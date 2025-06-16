
namespace Yu.Domain.Entities;

public record DeleteOrderCommand(int OrderId, int ReasonId) : IRequest;

internal class DeleteOrderCommandHandler(IYuDbContext dbContext) : IRequestHandler<DeleteOrderCommand>
{
    public async Task Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = await dbContext.Orders.FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        

        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}