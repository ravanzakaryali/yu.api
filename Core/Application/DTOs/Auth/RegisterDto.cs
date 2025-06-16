namespace Yu.Application.DTOs;

public class RegisterDto
{
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string ConfirmCode { get; set; } = null!;
}