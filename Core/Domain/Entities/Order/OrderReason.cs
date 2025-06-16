namespace Yu.Domain.Entities;

public class OrderReason : BaseAuditableEntity
{
    public OrderReason()
    {
        DeleteOrder = new HashSet<DeleteOrder>();
    }
    public string Name { get; set; } = null!;
    public ICollection<DeleteOrder> DeleteOrder { get; set; }
}