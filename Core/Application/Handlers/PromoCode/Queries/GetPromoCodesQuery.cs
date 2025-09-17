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
                .Include(pc => pc.Orders)
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

            // Date filtering logic
            if (request.Filter.StartDate.HasValue && request.Filter.EndDate.HasValue)
            {
                // Both dates provided - filter by date range
                query = query.Where(pc => pc.StartDate >= request.Filter.StartDate.Value && 
                                         pc.StartDate <= request.Filter.EndDate.Value);
            }
            else if (request.Filter.StartDate.HasValue)
            {
                // Only start date provided - from start date to now
                query = query.Where(pc => pc.StartDate >= request.Filter.StartDate.Value);
            }
            else if (request.Filter.EndDate.HasValue)
            {
                // Only end date provided - from beginning to end date
                query = query.Where(pc => pc.StartDate <= request.Filter.EndDate.Value);
            }
            // If no dates provided, show all records (no additional filtering)
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
                OrdersCount = pc.Orders.Count()
            })
            .ToListAsync(cancellationToken);

        return promoCodes;
    }
}