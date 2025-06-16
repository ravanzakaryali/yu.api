namespace Yu.Domain.Entities;

public class Price : BaseAuditableEntity
{
    public decimal Value { get; set; }
    public string Currency { get; set; } = null!;
    public string ServiceName { get; set; } = null!;
    public int ServiceId { get; set; }
    public Service Service { get; set; } = null!;
    public int? ClothingItemId { get; set; }
    public ClothingItem? ClothingItem { get; set; }
}