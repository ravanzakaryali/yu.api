namespace Yu.Application.DTOs;

public class CreatePromoCodeRequestDto
{
    public string Code { get; set; } = null!;
    public PromoCodeType Type { get; set; }
    public decimal Total { get; set; }
    public decimal? MinumumAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
} 