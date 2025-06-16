namespace Yu.Application.DTOs;

public class OrderRequestDto
{
    public string Comment { get; set; } = null!;
    public ICollection<int> Files { get; set; } = null!;
    public ICollection<OrderServiceDto> Services { get; set; } = null!;
}

public class OrderServiceDto
{
    public int ServiceId { get; set; }
    public int? Count { get; set; }
    public ICollection<int>? ClothingItemIds { get; set; } = null!;
}
