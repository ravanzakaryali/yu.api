namespace Yu.Application.DTOs;

public class TokenDto
{
    public string AccessToken { get; set; } = null!;
    public DateTime Expires { get; set; }
}
