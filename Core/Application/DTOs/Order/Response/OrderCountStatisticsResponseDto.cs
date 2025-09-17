namespace Yu.Application.DTOs;

public class OrderCountStatisticsResponseDto
{
    public int TotalOrders { get; set; }
    public int DeliveryOrders { get; set; }
    public int SelfDeliveryOrders { get; set; }
}
