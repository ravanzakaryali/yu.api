namespace Yu.Application.DTOs;

public class GetPromoCodesFilterRequestDto
{
    public bool? IsActive { get; set; }
    public PromoCodeType? Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
} 