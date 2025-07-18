namespace Yu.Domain.Entities;

public class PromoCode : BaseAuditableEntity
{
    public PromoCode()
    {
        Orders = new HashSet<Order>();
    }
    public string Code { get; set; } = null!;
    public PromoCodeType Type { get; set; }
    //price and procent
    public decimal Total { get; set; }
    public decimal? MinumumAmount { get; set; }
    public int? MaxUsageCount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public ICollection<Order> Orders { get; set; }
}