using Yu.Domain.Enums;

namespace Yu.Application.Handlers;

public record UpdateOrderDateCommand(int OrderId, DateTime Date, OrderDateType DateType) : IRequest;

internal class UpdateOrderDateCommandHandler(IYuDbContext yuDbContext) : IRequestHandler<UpdateOrderDateCommand>
{
    public async Task Handle(UpdateOrderDateCommand request, CancellationToken cancellationToken)
    {
        Order order = await yuDbContext.Orders
            .Include(o => o.PickupDateSetting)
            .Include(o => o.OrderStatusHistories)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        switch (request.DateType)
        {
            case OrderDateType.PickupDate:
                var orderPickupStatus = order.OrderStatusHistories.FirstOrDefault(osh => osh.OrderStatus == OrderStatus.PickUp);
                if (orderPickupStatus != null)
                {
                    orderPickupStatus.StatusDate = request.Date;
                }
                else
                {
                    order.OrderStatusHistories.Add(new OrderStatusHistory
                    {
                        OrderStatus = OrderStatus.PickUp,
                        SubStatus = Status.Pending,
                        StatusDate = request.Date,
                        Comment = "Pickup date changed for admin"
                    });
                }
                break;
            case OrderDateType.DeliveryDate:
                var orderDeliveryStatus = order.OrderStatusHistories.FirstOrDefault(osh => osh.OrderStatus == OrderStatus.Delivery);
                if (orderDeliveryStatus != null)
                {
                    orderDeliveryStatus.StatusDate = request.Date;
                }
                else
                {
                    order.OrderStatusHistories.Add(new OrderStatusHistory
                    {
                        OrderStatus = OrderStatus.Delivery,
                        SubStatus = Status.Pending,
                        StatusDate = request.Date,
                        Comment = "Delivery date changed for admin"
                    });
                }
                break;
            default:
                throw new ArgumentException("Invalid date type", nameof(request.DateType));
        }

        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
}