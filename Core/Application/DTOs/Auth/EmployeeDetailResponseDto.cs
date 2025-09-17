namespace Yu.Application.DTOs;

public class EmployeeDetailResponseDto
{
    public string Id { get; set; } = null!;
    public string Username { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
}
