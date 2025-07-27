namespace Yu.Domain.Entities;

public class Address : BaseAuditableEntity
{
    public bool IsDefault { get; set; }
    public string FullAddress { get; set; } = null!;
    public string SubDoor { get; set; } = null!;
    public string Floor { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public string Intercom { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public string Country { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public Member User { get; set; } = null!;
}
