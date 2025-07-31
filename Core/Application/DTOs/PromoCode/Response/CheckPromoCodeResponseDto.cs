namespace Yu.Application.DTOs;

public class CheckPromoCodeResponseDto
{
    public int? PromoCodeId { get; set; }
    public bool IsValid { get; set; }
    public string? Message { get; set; }
} 