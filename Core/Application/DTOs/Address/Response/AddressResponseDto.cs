namespace Yu.Application.DTOs;

public class AddressResponseDto
{
    public int Id { get; set; }
    public string FullAddress { get; set; } = null!;
    public string SubDoor { get; set; } = null!;
    public string Floor { get; set; } = null!;
    public string Apartment { get; set; } = null!;
    public string Intercom { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public string Country { get; set; } = null!;
    public bool IsDefault { get; set; }
    public DateTime CreatedDate { get; set; }
} 