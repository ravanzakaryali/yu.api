namespace Yu.Application.DTOs;

public class CheckPromoCodeResponseDto
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public PromoCodeType Type { get; set; }
}