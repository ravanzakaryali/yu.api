namespace Yu.Application.Handlers;

public record GetClothinItemsQuery : IRequest<List<ClothingItemResponseDto>>;

public class GetClothinItemsQueryHandler(IYuDbContext yuDbContext) : IRequestHandler<GetClothinItemsQuery, List<ClothingItemResponseDto>>
{
    public async Task<List<ClothingItemResponseDto>> Handle(GetClothinItemsQuery request, CancellationToken cancellationToken)
    {
        var clothingItems = await yuDbContext.ClothingItems
                    .Include(c => c.Price).Select(c => new ClothingItemResponseDto()
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

                    }).ToListAsync(cancellationToken);

        return clothingItems;
    }
}