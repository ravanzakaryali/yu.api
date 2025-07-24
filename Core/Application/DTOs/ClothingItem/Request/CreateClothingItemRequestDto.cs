namespace Yu.Application.DTOs;

public class CreateClothingItemRequestDto
{
    public string Name { get; set; } = null!;
    public double EstimateHours { get; set; }
    public decimal PriceValue { get; set; }
    public string? Currency { get; set; } = "AZN";
}