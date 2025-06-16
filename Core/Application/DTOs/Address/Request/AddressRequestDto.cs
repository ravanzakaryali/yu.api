namespace Yu.Application.DTOs;

public class AddressRequestDto
{
    public string City { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string House { get; set; } = null!;
    public string? Apartment { get; set; }
    public string? PostalCode { get; set; }

}