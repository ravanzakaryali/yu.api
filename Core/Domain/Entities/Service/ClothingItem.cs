namespace Yu.Domain.Entities;

public class ClothingItem : BaseAuditableEntity
{
    public ClothingItem()
    {
        Services = new HashSet<Service>();
    }
    public string Name { get; set; } = null!;
    public int PriceId { get; set; }
    public Price Price { get; set; } = null!;
    public ICollection<Service> Services { get; set; }
    public ICollection<OrderClothingItem> OrderClothingItems { get; set; } = null!;
}