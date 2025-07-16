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

        List<ActiveOrderResponseDto> activeOrders = await dbContext.Orders
            .Include(o => o.Services)
                .ThenInclude(os => os.Service)
            .Where(o => o.MemberId == userId)
            .Where(o => !o.OrderStatusHistories.Any(osh => osh.OrderStatus == OrderStatus.Completed))
            .Select(o => new ActiveOrderResponseDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CreatedDate = o.CreatedDate,
                OrderStatus = o.OrderStatusHistories.Last().OrderStatus,
                MainDescription = o.Comment,
                TotalPrice = o.TotalPrice,
                Services = o.Services.Select(os => new ActiveOrderServiceResponseDto
                {
                    ServiceName = os.Service.Title,
                    Count = os.Count
                }).ToList()
            })
            .ToListAsync(cancellationToken);

        return activeOrders;
    }
}