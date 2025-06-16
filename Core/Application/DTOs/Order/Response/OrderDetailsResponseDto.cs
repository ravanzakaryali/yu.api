using System.Globalization;

namespace Yu.Application.DTOs;

public class OrderDetailsResponseDto
{
    public OrderStatus Status { get; set; }
    public string Description { get; set; } = null!;
    public string MainDescription { get; set; } = null!;
}

public class OrderDetailServiceResponseDto
{
    public string ServiceName { get; set; } = null!;
    public int? Count { get; set; }
    public List<string>? ClothingItem { get; set; }
}