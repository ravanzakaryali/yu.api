namespace Yu.Application.DTOs;

public class OrderDetailsResponseDto
{
    public OrderDetailsResponseDto()
    {
        OrderStatusHistory = [];
    }
    public string Comment { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public string Description { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string MainDescription { get; set; } = null!;
    public ICollection<OrderStatusHistoryResponseDto> OrderStatusHistory { get; set; }
}

public class OrderStatusHistoryResponseDto
{
    public OrderStatus OrderStatus { get; set; }
    public Status SubStatus { get; set; }
}

public class OrderDetailServiceResponseDto
{
    public string ServiceName { get; set; } = null!;
    public int? Count { get; set; }
    public List<string>? ClothingItem { get; set; }
}