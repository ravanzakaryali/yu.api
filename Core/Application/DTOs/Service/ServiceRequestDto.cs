namespace Yu.Application.DTOs;

public class ServiceRequestDto
{
    public string Title { get; set; } = null!;
    public string SubTitle { get; set; } = null!;
    public string Desciption { get; set; } = null!;
    public ServiceType ServiceType { get; set; } = ServiceType.OnlyCount;
    public List<int> ImageIds { get; set; } = null!;
}