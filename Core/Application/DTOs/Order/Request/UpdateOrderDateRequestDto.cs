namespace Yu.Application.DTOs;

public class UpdateOrderDateRequestDto
{
    public DateTime Date { get; set; }
    public OrderDateType DateType { get; set; }
}