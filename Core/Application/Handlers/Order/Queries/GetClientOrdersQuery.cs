namespace Yu.Application.Handlers;

public record GetClientOrdersQuery(string ClientId) : IRequest<IEnumerable<OrderResponseDto>>;

internal class GetClientOrdersQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetClientOrdersQuery, IEnumerable<OrderResponseDto>>
{
    public async Task<IEnumerable<OrderResponseDto>> Handle(GetClientOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await dbContext.Orders
            .Include(o => o.Member)
            .Include(o => o.Address)
            .Include(o => o.OrderStatusHistories)
            .Where(o => o.MemberId == request.ClientId.ToString())
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderResponseDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,
                Comment = o.Comment,
                Address = o.Address.FullAddress,
                PaymentType = PaymentType.Online,
                TotalPrice = o.TotalPrice,
                User = new UserDto
                {
                    FullName = o.Member.FullName,
                    PhoneNumber = o.Member.PhoneNumber ?? string.Empty
                },
                OrderStatus = o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).FirstOrDefault() != null ?
                    o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).First().OrderStatus : OrderStatus.PickUp,
            })
            .ToListAsync(cancellationToken);

        return orders;
    }
}