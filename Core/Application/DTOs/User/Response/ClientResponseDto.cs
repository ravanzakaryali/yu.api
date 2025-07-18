namespace Yu.Application.DTOs;

public class ClientResponseDto
{
    public string Id { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public DateTime RegisterDate { get; set; }
    public string Phone { get; set; } = null!;
    public string? Email { get; set; }
    public int OrderCount { get; set; }
    public decimal TotalPrice { get; set; }
} 