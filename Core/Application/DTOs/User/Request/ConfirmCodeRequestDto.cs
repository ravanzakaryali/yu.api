namespace Yu.Application.DTOs;

public class ConfirmCodeRequest
{
    public string PhoneNumber { get; set; } = null!;
    public string Code { get; set; } = null!;
}