namespace Yu.Application.DTOs;

public class ClientPromoCodeResponseDto
{
    public int OrderId { get; set; }
    public string OrderNumber { get; set; } = null!;
    public string PromoCode { get; set; } = null!;
    public PromoCodeType Type { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime UsedDate { get; set; }
    public decimal OrderTotalPrice { get; set; }
} 