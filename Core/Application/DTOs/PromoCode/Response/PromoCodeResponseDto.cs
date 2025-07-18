namespace Yu.Application.DTOs;

public class PromoCodeResponseDto
{
    public string Code { get; set; } = null!;
    public PromoCodeType Type { get; set; }
    //price and procent
    public decimal Total { get; set; }
}
