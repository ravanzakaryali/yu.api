namespace Yu.Application.DTOs;

public class ErrorResponseDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = null!;
}
