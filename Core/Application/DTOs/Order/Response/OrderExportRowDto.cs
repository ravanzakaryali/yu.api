namespace Yu.Application.DTOs;

public class OrderExportRowDto
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime CreatedDate { get; set; }
    public string CustomerFullName { get; set; } = null!;
    public string CustomerPhone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public decimal TotalPrice { get; set; }
    public OrderStatus OrderStatus { get; set; }
    public Status SubStatus { get; set; }
}


