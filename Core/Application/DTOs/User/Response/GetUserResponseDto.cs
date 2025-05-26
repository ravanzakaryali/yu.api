namespace Yu.Application.DTOs;

public class GetUserResponseDto
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public IList<string> Roles { get; set; } = [];
}