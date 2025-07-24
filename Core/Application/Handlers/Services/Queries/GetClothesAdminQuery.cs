namespace Yu.Application.Handlers;

public record GetClothesAdminQuery : IRequest<List<ClothingItemAdminResponseDto>>;

public class GetClothesAdminQueryHandler(IYuDbContext yuDbContext) : IRequestHandler<GetClothesAdminQuery, List<ClothingItemAdminResponseDto>>
{
    public async Task<List<ClothingItemAdminResponseDto>> Handle(GetClothesAdminQuery request, CancellationToken cancellationToken)
    {
        return await yuDbContext.ClothingItems
            .IgnoreQueryFilters()
            .Where(c => !c.IsDeleted)
            .Include(c => c.Price)
            .Select(c => new ClothingItemAdminResponseDto()
            {
                Id = c.Id,
                Name = c.Name,
                EstimateHours = c.EstimateHours,
                Price = new PriceResponseDto()
                {
                    Id = c.Price.Id,
                    Value = c.Price.Value,
                    Currency = c.Price.Currency,
                },
                IsActive = c.IsActive,
            }).ToListAsync(cancellationToken);
    }
}