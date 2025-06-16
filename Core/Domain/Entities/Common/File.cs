namespace Yu.Domain.Entities;

public class File : BaseAuditableEntity
{
    public File()
    {
        Services = new HashSet<Service>();
        OrderImages = new HashSet<OrderImage>();
    }
    public string Name { get; set; } = null!;
    public string Path { get; set; } = null!;
    public string? ContentType { get; set; }
    public double Size { get; set; }
    public string? Extension { get; set; }
    public ICollection<Service> Services { get; set; }
    public ICollection<OrderImage> OrderImages { get; set; }
}