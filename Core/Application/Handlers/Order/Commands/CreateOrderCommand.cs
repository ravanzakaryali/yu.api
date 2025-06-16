namespace Yu.Application.Handlers;

public record CreateOrderCommand(string Comment, ICollection<int> Files, AddressRequestDto Address, ICollection<OrderServiceDto> Services) : IRequest<OrderResponseDto>;

internal class CreateOrderCommandHandler(IYuDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<CreateOrderCommand, OrderResponseDto>
{
    public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated");
        Member? member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Id == currentUserService.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found");
        Order order = new()
        {
            Comment = request.Comment,
            PickupDate = DateTime.UtcNow,
            Discount = 0,
            MemberId = member.Id,
            Address = new Address
            {
                City = request.Address.City,
                Country = "Az",
                UserId = member.Id,
                Street = request.Address.Street,
                House = request.Address.House,
                Apartment = request.Address.Apartment,
                PostalCode = request.Address.PostalCode
            }
        };

        List<Service> services = await dbContext.Services
            .Include(s => s.Price)
            .ToListAsync(cancellationToken);

        List<ClothingItem> clothingItems = await dbContext.ClothingItems.ToListAsync(cancellationToken);

        List<Domain.Entities.File> files = await dbContext.Files
            .Where(f => request.Files.Contains(f.Id))
            .ToListAsync(cancellationToken);

        if (files.Count != request.Files.Count)
            throw new NotFoundException("Some files not found");

        order.Images = [.. files.Select(f => new OrderImage
        {
            FileId = f.Id,
            Order = order,
        })];

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
            else if (service.ServiceType == ServiceType.ChooseType && requestService.ClothingItems is not null && requestService.ClothingItems.Count != 0)
            {
                List<ClothingItem> selectedItems = [.. clothingItems.Where(ci => requestService.ClothingItems.Select(c => c.ClothingItemId).Contains(ci.Id))];

                if (selectedItems.Count != requestService.ClothingItems.Count)
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
                OrderClothingItems = requestService.ClothingItems?.Select(ci => new OrderClothingItem
                {
                    ClothingItemId = ci.ClothingItemId,
                    Count = ci.Count,
                    OrderServiceId = requestService.ServiceId,
                    Order = order
                }).ToList() ?? []
            });
        }

        order.OrderStatusHistories.Add(new OrderStatusHistory
        {
            Comment = "Order created",
            OrderStatus = OrderStatus.PickUp,
            SubStatus = Status.Pending,
            StatusDate = DateTime.UtcNow.AddHours(4),
            Order = order
        });

        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        order.OrderNumber += order.Id.ToString("D6");

        await dbContext.SaveChangesAsync(cancellationToken);

        return new OrderResponseDto
        {
            Id = order.Id,
        };
    }
}