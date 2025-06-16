namespace Yu.Domain.Entities;

public class OrderClothingItem : BaseAuditableEntity
{
    public int OrderServiceId { get; set; }
    public OrderService OrderService { get; set; } = null!;
    public int ClothingItemId { get; set; }
    public ClothingItem ClothingItem { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}