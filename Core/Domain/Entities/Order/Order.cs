namespace Yu.Domain.Entities;

public class Order : BaseAuditableEntity
{
    public Order()
    {
        Services = new HashSet<OrderService>();
        DeleteOrders = new HashSet<DeleteOrder>();
        OrderClothingItems = new HashSet<OrderClothingItem>();
        OrderStatusHistories = new HashSet<OrderStatusHistory>();
    }
    public DateTime PickupDate { get; set; }
    public string OrderNumber { get; set; } = null!;
    public float Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public string Comment { get; set; } = null!;
    public int AddressId { get; set; }
    public Address Address { get; set; } = null!;
    public string MemberId { get; set; } = null!;
    public Member Member { get; set; } = null!;
    public ICollection<OrderImage> Images { get; set; } = null!;
    public ICollection<OrderService> Services { get; set; }
    public ICollection<DeleteOrder> DeleteOrders { get; set; }
    public ICollection<OrderStatusHistory> OrderStatusHistories { get; set; }
    public ICollection<OrderClothingItem> OrderClothingItems { get; set; }
}