namespace Yu.Application.DTOs;

public class GetUserResponseDto
{
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Email { get; set; }
    public IList<string> Roles { get; set; } = [];
}