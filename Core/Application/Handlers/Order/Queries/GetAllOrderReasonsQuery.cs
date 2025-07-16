namespace Yu.Application.Handlers;

public record GetAllOrderReasonsQuery : IRequest<IEnumerable<OrderReasonResponseDto>>;

internal class GetAllOrderReasonsQueryHandler(IYuDbContext yuDbContext) : IRequestHandler<GetAllOrderReasonsQuery, IEnumerable<OrderReasonResponseDto>>
{
    public async Task<IEnumerable<OrderReasonResponseDto>> Handle(GetAllOrderReasonsQuery request, CancellationToken cancellationToken)
    {
        var reasons = await yuDbContext.CancelOrderReasons
            .Where(r => r.IsActive && !r.IsDeleted)
            .Select(r => new OrderReasonResponseDto
            {
                Id = r.Id,
                Name = r.Name
            })
            .ToListAsync(cancellationToken);

        return reasons;
    }
} 