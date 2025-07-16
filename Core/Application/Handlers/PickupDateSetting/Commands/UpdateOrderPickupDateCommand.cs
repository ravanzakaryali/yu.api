namespace Yu.Application.Handlers;

public record UpdateOrderPickupDateCommand(int OrderId, int PickupDateSettingId) : IRequest;

internal class UpdateOrderPickupDateCommandHandler(IYuDbContext yuDbContext) : IRequestHandler<UpdateOrderPickupDateCommand>
{
    public async Task Handle(UpdateOrderPickupDateCommand request, CancellationToken cancellationToken)
    {
        Order order = await yuDbContext.Orders
            .Include(o => o.PickupDateSettingId)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        PickupDateSetting pickupDateSetting = await yuDbContext.PickupDateSettings
            .FirstOrDefaultAsync(pds => pds.Id == request.PickupDateSettingId, cancellationToken)
            ?? throw new NotFoundException(nameof(PickupDateSetting), request.PickupDateSettingId);

        order.PickupDateSetting = pickupDateSetting;

        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
}