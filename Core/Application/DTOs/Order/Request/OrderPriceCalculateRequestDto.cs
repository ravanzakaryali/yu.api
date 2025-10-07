namespace Yu.Application.DTOs;

public class OrderPriceCalculateRequestDto
{
    public ICollection<OrderServiceDto> Services { get; set; } = null!;
    public int? PromoCodeId { get; set; }
}