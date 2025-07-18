namespace Yu.Application.DTOs;

public class AddEmployeeRequestDto
{
    public string FullName { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
} 