namespace Yu.Domain.Entities;

public class OrderService : BaseAuditableEntity
{
    public OrderService()
    {
        Files = new HashSet<File>();
        OrderClothingItems = new HashSet<OrderClothingItem>();
    }
    public string ServiceName { get; set; } = null!;
    public Service Service { get; set; } = null!;
    public int ServiceId { get; set; }
    public Order Order { get; set; } = null!;
    public int OrderId { get; set; }
    public decimal Price { get; set; }
    public int? Count { get; set; }
    public ServiceType ServiceType { get; set; }
    public ICollection<File> Files { get; set; }
    public ICollection<OrderClothingItem> OrderClothingItems { get; set; }
}