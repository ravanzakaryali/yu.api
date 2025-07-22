namespace Yu.Domain.Entities;

public class Service : BaseAuditableEntity
{
    public Service()
    {
        Images = new HashSet<File>();
        ClothingItems = new HashSet<ClothingItem>();
    }
    public string Title { get; set; } = null!;
    public string SubTitle { get; set; } = null!;
    public string Tag { get; set; } = null!;
    public string TagTextColor { get; set; } = null!;
    public string TagBackgroundColor { get; set; } = null!;
    public int? IconId { get; set; }
    public File? Icon { get; set; }
    public int EstimatedDays { get; set; }
    public ServiceType ServiceType { get; set; }
    public ICollection<File> Images { get; set; }
    public int? PriceId { get; set; }
    public Price? Price { get; set; }
    public ICollection<ClothingItem> ClothingItems { get; set; }
}