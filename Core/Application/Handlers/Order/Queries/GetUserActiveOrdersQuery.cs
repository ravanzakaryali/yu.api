namespace Yu.Application.Handlers;

public record GetUserActiveOrdersQuery() : IRequest<IEnumerable<ActiveOrderResponseDto>>;

internal class GetUserActiveOrdersQueryHandler(IYuDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<GetUserActiveOrdersQuery, IEnumerable<ActiveOrderResponseDto>>
{
    public async Task<IEnumerable<ActiveOrderResponseDto>> Handle(GetUserActiveOrdersQuery request, CancellationToken cancellationToken)
    {
        string? userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new AutheticationException("User not authenticated");

        var activeOrders = await dbContext.Orders
            .Include(o => o.Services)
                .ThenInclude(os => os.Service)
            .Where(o => o.MemberId == userId)
            .Where(o => !o.OrderStatusHistories.Any(osh => osh.OrderStatus == OrderStatus.Completed))

            .ToListAsync(cancellationToken);

        return activeOrders.Select(o =>
        {
            var latestStatus = o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).FirstOrDefault();
            return new ActiveOrderResponseDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,
                OrderStatus = latestStatus != null ? latestStatus.OrderStatus : OrderStatus.PickUp,
                SubStatus = latestStatus != null ? latestStatus.SubStatus : Status.Pending,
                Description = o.Comment,
                TotalPrice = o.TotalPrice,
                Services = o.Services.Select(os => new ActiveOrderServiceResponseDto
                {
                    ServiceName = os.Service.Title,
                    Count = os.Count
                }).ToList()
            };
        });
    }
}