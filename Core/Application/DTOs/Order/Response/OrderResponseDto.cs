namespace Yu.Application.DTOs;

public class OrderResponseDto
{
    public int Id { get; set; }
    public UserDto User { get; set; } = null!;
    public string OrderNumber { get; set; } = null!;
    public string Address { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string Comment { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public PaymentType PaymentType { get; set; }
}