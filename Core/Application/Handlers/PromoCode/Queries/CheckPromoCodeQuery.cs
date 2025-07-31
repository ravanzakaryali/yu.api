namespace Yu.Application.Handlers;

public record CheckPromoCodeQuery(string Code) : IRequest<CheckPromoCodeResponseDto>;

internal class CheckPromoCodeQueryHandler(IYuDbContext dbContext) : IRequestHandler<CheckPromoCodeQuery, CheckPromoCodeResponseDto>
{
    public async Task<CheckPromoCodeResponseDto> Handle(CheckPromoCodeQuery request, CancellationToken cancellationToken)
    {
        var promoCode = await dbContext.PromoCodes
            .FirstOrDefaultAsync(pc => pc.Code == request.Code, cancellationToken);

        if (promoCode == null)
            throw new NotFoundException("Promo code not found");

        var currentDate = DateTime.UtcNow;

        if (promoCode.StartDate > currentDate)
            throw new NotFoundException("Promo code is not yet active");

        if (promoCode.EndDate.HasValue && promoCode.EndDate.Value < currentDate)
            throw new NotFoundException("Promo code has expired");

        return new CheckPromoCodeResponseDto
        {
            Id = promoCode.Id,
            Total = promoCode.Total,
            Type = promoCode.Type
        };
    }
}