using Yu.Application.Abstractions;
using Yu.Application.DTOs;
using Yu.Domain.Entities;

namespace Yu.Application.Handlers;

public record GetPromoCodesQuery(GetPromoCodesFilterRequestDto? Filter = null) : IRequest<List<PromoCodeResponseDto>>;

internal class GetPromoCodesQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetPromoCodesQuery, List<PromoCodeResponseDto>>
{
    public async Task<List<PromoCodeResponseDto>> Handle(GetPromoCodesQuery request, CancellationToken cancellationToken)
    {
        IQueryable<PromoCode> query = dbContext.PromoCodes
                .IgnoreQueryFilters()
                .Where(pc => !pc.IsDeleted)
                .AsQueryable();

        if (request.Filter != null)
        {
            if (request.Filter.IsActive.HasValue)
            {
                query = query.Where(pc => pc.IsActive == request.Filter.IsActive.Value);
            }

            if (request.Filter.Type.HasValue)
            {
                query = query.Where(pc => pc.Type == request.Filter.Type.Value);
            }

            if (request.Filter.StartDate.HasValue)
            {
                query = query.Where(pc => pc.StartDate >= request.Filter.StartDate.Value);
            }

            if (request.Filter.EndDate.HasValue)
            {
                query = query.Where(pc => pc.EndDate <= request.Filter.EndDate.Value);
            }
        }

        List<PromoCodeResponseDto> promoCodes = await query
            .Select(pc => new PromoCodeResponseDto
            {
                Id = pc.Id,
                Code = pc.Code,
                Type = pc.Type,
                Total = pc.Total,
                MinumumAmount = pc.MinumumAmount,
                MaxUsageCount = pc.MaxUsageCount,
                StartDate = pc.StartDate,
                EndDate = pc.EndDate,
                IsActive = pc.IsActive,
                CreatedDate = pc.CreatedDate,
            })
            .ToListAsync(cancellationToken);

        return promoCodes;
    }
}