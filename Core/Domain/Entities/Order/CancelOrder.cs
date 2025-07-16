namespace Yu.Domain.Entities;

public class CancelOrder : BaseAuditableEntity
{
    public int ReasonId { get; set; }
    public CancelOrderReason Reason { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}