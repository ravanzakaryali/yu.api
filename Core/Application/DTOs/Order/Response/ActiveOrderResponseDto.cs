namespace Yu.Application.DTOs;

public class ActiveOrderResponseDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string Description { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Status SubStatus { get; set; }
    public ICollection<ActiveOrderServiceResponseDto> Services { get; set; } = null!;
}

public class ActiveOrderServiceResponseDto
{
    public string ServiceName { get; set; } = null!;
    public int? Count { get; set; }
}