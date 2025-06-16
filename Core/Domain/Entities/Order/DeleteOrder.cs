namespace Yu.Domain.Entities;

public class DeleteOrder : BaseAuditableEntity
{
    public int ReasonId { get; set; }
    public OrderReason Reason { get; set; } = null!;
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}