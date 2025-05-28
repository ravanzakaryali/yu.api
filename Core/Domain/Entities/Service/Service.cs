namespace Yu.Domain.Entities;

public class Service : BaseAuditableEntity
{
    public Service()
    {
        Images = new HashSet<File>();
    }
    public string Title { get; set; } = null!;
    public string SubTitle { get; set; } = null!;
    public string Desciption { get; set; } = null!;
    public int EstimatedDays { get; set; }
    public ICollection<File> Images { get; set; }
}