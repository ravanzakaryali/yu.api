namespace Yu.Domain.Entities;

public class OrderImage : BaseAuditableEntity
{
    public int FileId { get; set; }
    public File File { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}