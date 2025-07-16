namespace Yu.Domain.Entities;

public class CancelOrderReason : BaseAuditableEntity
{
    public CancelOrderReason()
    {
        CancelOrder = new HashSet<CancelOrder>();
    }
    public string Name { get; set; } = null!;
    public ICollection<CancelOrder> CancelOrder { get; set; }
}