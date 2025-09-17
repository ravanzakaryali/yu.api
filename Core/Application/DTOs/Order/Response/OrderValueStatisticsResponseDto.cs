namespace Yu.Application.DTOs;

public class OrderValueStatisticsResponseDto
{
    public decimal TotalValue { get; set; }
    public int OnlineOrders { get; set; }
    public int OfflineOrders { get; set; }
}
