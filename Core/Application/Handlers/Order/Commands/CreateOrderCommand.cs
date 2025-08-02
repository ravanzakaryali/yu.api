namespace Yu.Application.Handlers;

public record CreateOrderCommand(string Comment, ICollection<int> Files, AddressRequestDto Address, ICollection<OrderServiceDto> Services, int? PromoCodeId) : IRequest<OrderResponseDto>;

internal class CreateOrderCommandHandler(IYuDbContext dbContext, ICurrentUserService currentUserService) : IRequestHandler<CreateOrderCommand, OrderResponseDto>
{
    public async Task<OrderResponseDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        if (currentUserService.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated");

        Member? member = await dbContext.Members
            .FirstOrDefaultAsync(m => m.Id == currentUserService.UserId, cancellationToken)
            ?? throw new UnauthorizedAccessException("User not found");


        // check promo code

        Order order = new()
        {
            Comment = request.Comment,
            PickupDate = DateTime.UtcNow,
            Discount = 0,
            PromoCodeId = request.PromoCodeId,
            MemberId = member.Id,
            Address = new Address
            {
                FullAddress = request.Address.FullAddress,
                SubDoor = request.Address.SubDoor,
                Floor = request.Address.Floor,
                Apartment = request.Address.Apartment,
                Intercom = request.Address.Intercom,
                Comment = request.Address.Comment,
                UserId = member.Id,
                Country = "Az"
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

        order.Images = [..files.Select(f => new OrderImage
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

        // Algorithm to find the best pickup date
        // Get all pickup date settings
        List<PickupDateSetting> pickupDateSettings = await dbContext.PickupDateSettings
            .ToListAsync(cancellationToken);

        if (pickupDateSettings.Count == 0)
            throw new NotFoundException("No pickup date settings found");

        // Get current date and time
        DateTime currentDateTime = DateTime.UtcNow;
        DateTime currentDate = currentDateTime.Date;

        // Find the best pickup date
        PickupDateSetting? bestPickupDate = null;
        DateTime? bestPickupDateTime = null;

        // Simple approach: Get all pickup settings and find the first available one
        var allPickupSettings = pickupDateSettings.OrderBy(p => p.DayOfWeek).ThenBy(p => p.StartTime).ToList();

        // For debugging, let's first try to find ANY pickup setting for today
        var todaySettings = allPickupSettings.Where(p => p.DayOfWeek == currentDate.DayOfWeek).ToList();

        if (todaySettings.Count > 0)
        {
            // Take the first available setting for today
            var firstTodaySetting = todaySettings.First();
            DateTime pickupDateTime = currentDate.Add(firstTodaySetting.StartTime.ToTimeSpan());

            // Only skip if the time has already passed
            if (pickupDateTime > currentDateTime)
            {
                bestPickupDate = firstTodaySetting;
                bestPickupDateTime = pickupDateTime;
            }
        }

        // If no pickup for today, look for the next available one
        if (bestPickupDate == null)
        {
            // Look for pickup dates starting from tomorrow and going forward for 4 weeks
            for (int dayOffset = 1; dayOffset <= 28; dayOffset++) // Start from tomorrow
            {
                DateTime targetDate = currentDate.AddDays(dayOffset);
                DayOfWeek targetDayOfWeek = targetDate.DayOfWeek;

                // Find pickup settings for this day of week
                var dayPickupSettings = allPickupSettings
                    .Where(p => p.DayOfWeek == targetDayOfWeek)
                    .OrderBy(p => p.StartTime)
                    .ToList();

                if (dayPickupSettings.Count > 0)
                {
                    // Take the first available setting for this day
                    var firstSetting = dayPickupSettings.First();
                    DateTime pickupDateTime = targetDate.Add(firstSetting.StartTime.ToTimeSpan());

                    bestPickupDate = firstSetting;
                    bestPickupDateTime = pickupDateTime;
                    break;
                }
            }
        }

        if (bestPickupDate == null || bestPickupDateTime == null)
            throw new NotFoundException($"No available pickup date found within the next 4 weeks. Total settings: {pickupDateSettings.Count}");

        order.PickupDate = bestPickupDateTime.Value;
        order.PickupDateSetting = bestPickupDate;
        order.PickupDateSettingId = bestPickupDate.Id;

        order.OrderStatusHistories.Add(new OrderStatusHistory
        {
            Comment = "Order created",
            OrderStatus = OrderStatus.PickUp,
            SubStatus = Status.Pending,
            StatusDate = DateTime.UtcNow,
            Order = order
        });

        await dbContext.Orders.AddAsync(order, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        order.OrderNumber += "-" + order.Id.ToString("D6");

        await dbContext.SaveChangesAsync(cancellationToken);

        return new OrderResponseDto
        {
            Id = order.Id,
        };
    }
}