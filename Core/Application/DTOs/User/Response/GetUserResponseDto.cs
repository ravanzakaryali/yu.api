namespace Yu.Application.DTOs;

public class GetUserResponseDto
{
    public string FullName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public IList<string> Roles { get; set; } = [];
}