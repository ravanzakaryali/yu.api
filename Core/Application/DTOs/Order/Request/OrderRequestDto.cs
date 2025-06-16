namespace Yu.Application.DTOs;

public class OrderRequestDto
{
    public string Comment { get; set; } = null!;
    public ICollection<int> Files { get; set; } = null!;
    public AddressRequestDto Address { get; set; } = null!;
    public ICollection<OrderServiceDto> Services { get; set; } = null!;
}

public class OrderServiceDto
{
    public int ServiceId { get; set; }
    public int? Count { get; set; }
    public ICollection<OrderClothingItemRequestDto>? ClothingItems { get; set; }
}

public class OrderClothingItemRequestDto
{
    public int ClothingItemId { get; set; }
    public int Count { get; set; }
}


