namespace Yu.Application.Handlers;

public record GetUserArchivedOrdersQuery() : IRequest<IEnumerable<ActiveOrderResponseDto>>;

internal class GetUserArchivedOrdersQueryHandler(IYuDbContext dbContext, ICurrentUserService currentUserService)
    : IRequestHandler<GetUserArchivedOrdersQuery, IEnumerable<ActiveOrderResponseDto>>
{
    public async Task<IEnumerable<ActiveOrderResponseDto>> Handle(GetUserArchivedOrdersQuery request, CancellationToken cancellationToken)
    {
        string? userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
            throw new AutheticationException("User not authenticated");

        var archivedOrders = await dbContext.Orders
            .Include(o => o.Address)
            .Include(o => o.Services)
                .ThenInclude(os => os.Service)
            .Include(o => o.Member)
            .Include(o => o.OrderStatusHistories)
            .Where(o => o.MemberId == userId)
            .OrderByDescending(o => o.CreatedDate)
            .ToListAsync(cancellationToken);

        return archivedOrders.Select(o =>
        {
            var latestStatus = o.OrderStatusHistories.OrderByDescending(osh => osh.CreatedDate).FirstOrDefault();
            return new ActiveOrderResponseDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,
                Description = o.Comment,
                TotalPrice = o.TotalPrice,
                OrderStatus = latestStatus != null ? latestStatus.OrderStatus : OrderStatus.Completed,
                SubStatus = latestStatus != null ? latestStatus.SubStatus : Status.Pending,
                Services = o.Services.Select(os => new ActiveOrderServiceResponseDto
                {
                    ServiceName = os.Service.Title,
                    Count = os.Count
                }).ToList()
            };
        });
    }
}