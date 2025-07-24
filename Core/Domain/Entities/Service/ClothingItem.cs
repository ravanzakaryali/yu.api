namespace Yu.Domain.Entities;

public class ClothingItem : BaseAuditableEntity
{
    public string Name { get; set; } = null!;
    public int PriceId { get; set; }
    public double EstimateHours { get; set; }
    public Price Price { get; set; } = null!;
    public ICollection<OrderClothingItem> OrderClothingItems { get; set; } = null!;
}