namespace Yu.Domain.Entities;

public class Address : BaseAuditableEntity
{
    public string Street { get; set; } = null!;
    public string City { get; set; } = null!;
    public string House { get; set; } = null!;
    public string? Apartment { get; set; }
    public string? PostalCode { get; set; }
    public string Country { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public Member User { get; set; } = null!;
}
