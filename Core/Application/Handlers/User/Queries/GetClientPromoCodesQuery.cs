namespace Yu.Application.Handlers;

public record GetClientPromoCodesQuery(string ClientId) : IRequest<IEnumerable<ClientPromoCodeResponseDto>>;

internal class GetClientPromoCodesQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetClientPromoCodesQuery, IEnumerable<ClientPromoCodeResponseDto>>
{
    public async Task<IEnumerable<ClientPromoCodeResponseDto>> Handle(GetClientPromoCodesQuery request, CancellationToken cancellationToken)
    {
        var clientPromoCodes = await dbContext.Orders
            .Include(o => o.PromoCode)
            .Where(o => o.MemberId == request.ClientId && o.PromoCode != null)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new ClientPromoCodeResponseDto
            {
                OrderId = o.Id,
                OrderNumber = o.OrderNumber,
                PromoCode = o.PromoCode!.Code,
                Type = o.PromoCode.Type,
                DiscountAmount = o.PromoCode.Type == PromoCodeType.Procent 
                    ? (o.TotalPrice * o.PromoCode.Total / 100)
                    : o.PromoCode.Total,
                UsedDate = o.CreatedDate,
                OrderTotalPrice = o.TotalPrice
            })
            .ToListAsync(cancellationToken);

        return clientPromoCodes;
    }
} 