namespace Yu.Domain.Entities;

public class OrderStatusHistory : BaseAuditableEntity
{
    public OrderStatus OrderStatus { get; set; }
    public Status SubStatus { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public DateTime StatusDate { get; set; }
    public string? Comment { get; set; } = null!;
}