namespace Yu.Application.DTOs;

public class UpdateAddressRequestDto
{
    public string FullAddress { get; set; } = null!;
    public string Street { get; set; } = null!;
    public string SubDoor { get; set; } = null!;
    public string Floor { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public string Intercom { get; set; } = null!;
    public string Comment { get; set; } = null!;
} 