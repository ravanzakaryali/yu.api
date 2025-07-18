namespace Yu.Application.Handlers;

public record GetAdminOrderDetailsQuery(int OrderId) : IRequest<OrderDetailsAdminResponseDto>;

internal class GetAdminOrderDetailsQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetAdminOrderDetailsQuery, OrderDetailsAdminResponseDto>
{
    public async Task<OrderDetailsAdminResponseDto> Handle(GetAdminOrderDetailsQuery request, CancellationToken cancellationToken)
    {
        Order order = await dbContext.Orders
            .Include(o => o.Services)
                .ThenInclude(os => os.OrderClothingItems)
                    .ThenInclude(oci => oci.ClothingItem)
            .Include(o => o.OrderStatusHistories)
            .Include(o => o.Images)
                .ThenInclude(oi => oi.File)
            .Include(o => o.PromoCode)
            .FirstOrDefaultAsync(o => o.Id == request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        List<string> orderStatus = Enum.GetNames<OrderStatus>().ToList();

        List<OrderStatusHistoryResponseDto> orderStatusHistory = [.. order.OrderStatusHistories
                .Select(osh => new OrderStatusHistoryResponseDto
                {
                    OrderStatus = osh.OrderStatus,
                    SubStatus = osh.SubStatus
                })];

        foreach (var status in orderStatus)
        {
            if (!orderStatusHistory.Any(osh => osh.OrderStatus == Enum.Parse<OrderStatus>(status)))
            {
                orderStatusHistory.Add(new OrderStatusHistoryResponseDto
                {
                    OrderStatus = Enum.Parse<OrderStatus>(status),
                    SubStatus = null,
                });
            }
        }

        List<OrderDetailServiceResponseDto> services = [.. order.Services.Select(os => new OrderDetailServiceResponseDto
        {
            ServiceName = os.ServiceName,
            Count = os.Count,
            ClothingItem = os.OrderClothingItems?.Select(oci => oci.ClothingItem.Name).ToList()
        })];

        OrderDetailsAdminResponseDto orderResponse = new()
        {
            OrderNumber = order.OrderNumber,
            Comment = order.Comment,
            TotalPrice = order.TotalPrice,
            OrderStatusHistory = orderStatusHistory,
            Services = services,
            Images = order.Images.Select(i => i.File.Path).ToList(),
            PromoCode = order.PromoCode != null ? new PromoCodeResponseDto
            {
                Code = order.PromoCode.Code,
                Type = order.PromoCode.Type,
                Total = order.PromoCode.Total
            } : null
        };

        return orderResponse;
    }
}