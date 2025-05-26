namespace Yu.Application.DTOs;

public class LoginResponseDto
{
    public bool IsRegistered { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public int Count { get; set; }
}