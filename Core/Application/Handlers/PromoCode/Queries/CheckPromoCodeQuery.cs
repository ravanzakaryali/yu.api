using Yu.Application.Abstractions;
using Yu.Application.DTOs;
using Yu.Domain.Entities;

namespace Yu.Application.Handlers;

public record CheckPromoCodeQuery(string Code) : IRequest<CheckPromoCodeResponseDto>;

internal class CheckPromoCodeQueryHandler(IYuDbContext dbContext) : IRequestHandler<CheckPromoCodeQuery, CheckPromoCodeResponseDto>
{
    public async Task<CheckPromoCodeResponseDto> Handle(CheckPromoCodeQuery request, CancellationToken cancellationToken)
    {
        var promoCode = await dbContext.PromoCodes
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(pc => pc.Code == request.Code && !pc.IsDeleted, cancellationToken);

        if (promoCode == null)
        {
            return new CheckPromoCodeResponseDto
            {
                PromoCodeId = null,
                IsValid = false,
                Message = "Promo code not found"
            };
        }

        if (!promoCode.IsActive)
        {
            return new CheckPromoCodeResponseDto
            {
                PromoCodeId = null,
                IsValid = false,
                Message = "Promo code is not active"
            };
        }

        var currentDate = DateTime.UtcNow;
        if (promoCode.StartDate > currentDate)
        {
            return new CheckPromoCodeResponseDto
            {
                PromoCodeId = null,
                IsValid = false,
                Message = "Promo code is not yet active"
            };
        }

        if (promoCode.EndDate.HasValue && promoCode.EndDate.Value < currentDate)
        {
            return new CheckPromoCodeResponseDto
            {
                PromoCodeId = null,
                IsValid = false,
                Message = "Promo code has expired"
            };
        }

        return new CheckPromoCodeResponseDto
        {
            PromoCodeId = promoCode.Id,
            IsValid = true,
            Message = "Promo code is valid"
        };
    }
} 