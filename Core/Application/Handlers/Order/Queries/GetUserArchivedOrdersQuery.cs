namespace Yu.Application.Handlers;

public record GetUserArchivedOrdersQuery() : IRequest<IEnumerable<OrderResponseDto>>;

internal class GetUserArchivedOrdersQueryHandler(IYuDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<GetUserArchivedOrdersQuery, IEnumerable<OrderResponseDto>>
{
    public async Task<IEnumerable<OrderResponseDto>> Handle(GetUserArchivedOrdersQuery request, CancellationToken cancellationToken)
    {
        string? userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new AutheticationException("User not authenticated");

        var archivedOrders = await dbContext.Orders
            .Include(o => o.Address)
            .Include(o => o.Member)
            .Include(o => o.OrderStatusHistories)
            .Where(o => o.MemberId == userId)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);

        return archivedOrders.Select(o =>
        {
            var latestStatus = o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).FirstOrDefault();
            return new OrderResponseDto
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
                OrderStatus = latestStatus != null ? latestStatus.OrderStatus : OrderStatus.PickUp,
                SubStatus = latestStatus != null ? latestStatus.SubStatus : Status.Pending,
            };
        });
    }
} 