namespace Yu.Application.Handlers;

public record CreateClothingItemCommand(CreateClothingItemRequestDto Request) : IRequest<ClothingItemAdminResponseDto>;

internal class CreateClothingItemCommandHandler(IYuDbContext dbContext) : IRequestHandler<CreateClothingItemCommand, ClothingItemAdminResponseDto>
{
    public async Task<ClothingItemAdminResponseDto> Handle(CreateClothingItemCommand request, CancellationToken cancellationToken)
    {
        var exists = await dbContext.ClothingItems
            .AnyAsync(ci => ci.Name == request.Request.Name && !ci.IsDeleted, cancellationToken);

        if (exists)
            throw new AlreadyExistsException($"'{request.Request.Name}' has already been created");


        var clothingItem = new ClothingItem
        {
            Name = request.Request.Name,
            EstimateHours = request.Request.EstimateHours,
            Price = new Price
            {
                Value = request.Request.PriceValue,
                Currency = request.Request.Currency ?? "AZN",
            }
        };

        var clothingItemEntry = await dbContext.ClothingItems.AddAsync(clothingItem, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new ClothingItemAdminResponseDto
        {
            Id = clothingItemEntry.Entity.Id,
            Name = clothingItem.Name,
            EstimateHours = clothingItem.EstimateHours,
            IsActive = clothingItem.IsActive,
            Price = new PriceResponseDto
            {
                Id = clothingItem.Price.Id,
                Value = clothingItem.Price.Value,
                Currency = clothingItem.Price.Currency
            }
        };
    }
}