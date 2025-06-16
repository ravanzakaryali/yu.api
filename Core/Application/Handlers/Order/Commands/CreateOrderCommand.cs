namespace Yu.Application.Handlers;

public record CreateOrderCommand(string Comment, ICollection<int> Files, ICollection<OrderServiceDto> Services) : IRequest<OrderResponseDto>;

internal class CreateOrderCommandHandler(IYuDbContext dbContext) : IRequestHandler<CreateOrderCommand, OrderResponseDto>
{
    public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        Order order = new()
        {
            Comment = request.Comment,
            PickupDate = DateTime.UtcNow,
            Discount = 0,
        };

        List<Service> services = await dbContext.Services
            .Include(s => s.Price)
            .ToListAsync(cancellationToken);

        List<ClothingItem> clothingItems = await dbContext.ClothingItems.ToListAsync(cancellationToken);



        foreach (var requestService in request.Services)
        {
            decimal price = 0;

            Service? service = services.FirstOrDefault(s => s.Id == requestService.ServiceId)
                ?? throw new NotFoundException($"Service with ID {requestService.ServiceId} not found");

            if (service.ServiceType == ServiceType.OnlyCount && service.Price is not null)
            {
                order.TotalPrice += service.Price.Value * (requestService.Count ?? 1);
                price = service.Price.Value * (requestService.Count ?? 1);
            }
            else if (service.ServiceType == ServiceType.ChooseType && requestService.ClothingItemIds is not null && requestService.ClothingItemIds.Any())
            {
                List<ClothingItem> selectedItems = [.. clothingItems.Where(ci => requestService.ClothingItemIds.Contains(ci.Id))];

                if (selectedItems.Count != requestService.ClothingItemIds.Count)
                    throw new NotFoundException("Some clothing items not found");

                order.TotalPrice += selectedItems.Sum(ci => ci.Price.Value);
                price = selectedItems.Sum(ci => ci.Price.Value);
            }

            order.Services.Add(new OrderService
            {
                ServiceId = service.Id,
                Count = requestService.Count,
                Price = price,
                ServiceName = service.Title,
                ServiceType = service.ServiceType,
            });
        }

        if (services.Count != request.Services.Count)
            throw new NotFoundException("Some services not found");

        return new OrderResponseDto
        {
            Id = order.Id,
        };
    }
}