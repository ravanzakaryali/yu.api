namespace Yu.Application.DTOs;

public class UpdateEmployeeRequestDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
} 