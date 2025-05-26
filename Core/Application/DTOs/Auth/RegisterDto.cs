namespace Yu.Application.DTOs;

public class RegisterDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string ConfirmCode { get; set; } = null!;
}