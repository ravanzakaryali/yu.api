namespace Yu.Application.DTOs;

public class ClothingItemResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public double EstimateHours { get; set; }
    public PriceResponseDto? Price { get; set; }
}
public class ClothingItemAdminResponseDto : ClothingItemResponseDto
{
    public bool IsActive { get; set; }
}