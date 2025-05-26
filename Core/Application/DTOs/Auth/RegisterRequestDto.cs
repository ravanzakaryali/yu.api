namespace Yu.Application.DTOs;

public class RegisterRequestDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
}