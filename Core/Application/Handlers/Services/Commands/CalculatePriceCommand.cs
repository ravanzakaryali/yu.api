namespace Yu.Application.Handlers;

public record CalculatePriceCommand(ICollection<OrderServiceDto> Services) : IRequest<OrderPriceCalculateResponseDto>
{
	public int? PromoCodeId { get; init; }
}

internal class CalculatePriceCommandHandler(IYuDbContext dbContext) : IRequestHandler<CalculatePriceCommand, OrderPriceCalculateResponseDto>
{
	public async Task<OrderPriceCalculateResponseDto> Handle(CalculatePriceCommand request, CancellationToken cancellationToken)
	{
		decimal totalPrice = 0;

		// Preload necessary data to avoid N+1 queries
		List<Service> services = await dbContext.Services
			.Include(s => s.Price)
			.ToListAsync(cancellationToken);

		List<ClothingItem> clothingItems = await dbContext.ClothingItems
                        .Include(c => c.Price)
                        .ToListAsync(cancellationToken);

		foreach (var requestService in request.Services)
		{
			Service? service = services.FirstOrDefault(s => s.Id == requestService.ServiceId)
				?? throw new NotFoundException($"Service with ID {requestService.ServiceId} not found");

			if (service.ServiceType == ServiceType.OnlyCount && service.Price is not null)
			{
				int count = requestService.Count ?? 1;
				totalPrice += service.Price.Value * count;
			}
			else if (service.ServiceType == ServiceType.ChooseType && requestService.ClothingItems is not null && requestService.ClothingItems.Count != 0)
			{
				List<int> requestedItemIds = requestService.ClothingItems.Select(c => c.ClothingItemId).ToList();
				List<ClothingItem> selectedItems = clothingItems.Where(ci => requestedItemIds.Contains(ci.Id)).ToList();

				if (selectedItems.Count != requestService.ClothingItems.Count)
					throw new NotFoundException("Some clothing items not found");

				totalPrice += selectedItems.Sum(ci => ci.Price.Value);
			}
		}

		decimal oldPrice = totalPrice;

		// Apply promo code if provided
		if (request.PromoCodeId.HasValue)
		{
			var promoCode = await dbContext.PromoCodes.FirstOrDefaultAsync(pc => pc.Id == request.PromoCodeId.Value, cancellationToken)
				?? throw new NotFoundException("Promo code not found");

			var now = DateTime.UtcNow;
			if (promoCode.StartDate > now)
				throw new NotFoundException("Promo code is not yet active");
			if (promoCode.EndDate.HasValue && promoCode.EndDate.Value < now)
				throw new NotFoundException("Promo code has expired");

			// Enforce minimum amount if defined
			if (promoCode.MinumumAmount.HasValue && totalPrice >= promoCode.MinumumAmount.Value)
			{
				decimal discount = promoCode.Type == PromoCodeType.Procent
					? (totalPrice * promoCode.Total / 100)
					: promoCode.Total;
				totalPrice = Math.Max(0, totalPrice - discount);
			}
		}

		return new OrderPriceCalculateResponseDto
		{
			CurrentPrice = totalPrice,
			OldPrice = oldPrice
		};
	}
}


