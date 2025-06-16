namespace Yu.Domain.Entities;

public class Address : BaseAuditableEntity
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string House { get; set; } = null!;
    public string? PostalCode { get; set; }
    public string Country { get; set; } = null!;
    public int UserId { get; set; }
    public Member User { get; set; } = null!;
}
