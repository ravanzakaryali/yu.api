namespace Yu.Application.Handlers;

public record CreatePromoCodeCommand(CreatePromoCodeRequestDto Request) : IRequest<PromoCodeResponseDto>;

internal class CreatePromoCodeCommandHandler(IYuDbContext dbContext) : IRequestHandler<CreatePromoCodeCommand, PromoCodeResponseDto>
{
    public async Task<PromoCodeResponseDto> Handle(CreatePromoCodeCommand request, CancellationToken cancellationToken)
    {
        bool exists = await dbContext.PromoCodes
            .AnyAsync(pc => pc.Code == request.Request.Code, cancellationToken);

        if (exists)
        {
            throw new AlreadyExistsException($"Promo code '{request.Request.Code}' already exists");
        }

        PromoCode promoCode = new()
        {
            Code = request.Request.Code,
            Type = request.Request.Type,
            Total = request.Request.Total,
            MinumumAmount = request.Request.MinumumAmount,
            MaxUsageCount = request.Request.MaxUsageCount,
            StartDate = request.Request.StartDate,
            EndDate = request.Request.EndDate,
        };

        dbContext.PromoCodes.Add(promoCode);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new PromoCodeResponseDto
        {
            Id = promoCode.Id,
            Code = promoCode.Code,
            Type = promoCode.Type,
            Total = promoCode.Total,
            MinumumAmount = promoCode.MinumumAmount,
            MaxUsageCount = promoCode.MaxUsageCount,
            StartDate = promoCode.StartDate,
            EndDate = promoCode.EndDate,
            CreatedDate = promoCode.CreatedDate
        };
    }
} 