namespace Yu.Domain.Entities;

public class PickupDateSetting : BaseAuditableEntity
{
    public PickupDateSetting()
    {
        Orders = new HashSet<Order>();
    }
    // Sunday = 0, Monday = 1, ..., Saturday = 6
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string? Description { get; set; }
    public ICollection<Order> Orders { get; set; }
}