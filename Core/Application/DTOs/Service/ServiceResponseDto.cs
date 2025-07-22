namespace Yu.Application.DTOs;

public class ServiceResponseDto
{
    public ServiceResponseDto()
    {
        Images = [];
    }
    public int Id { get; set; }
    public string Title { get; set; } = null!;
    public string Tag { get; set; } = null!;
    public string SubTitle { get; set; } = null!;
    public string TagTextColor { get; set; } = null!;
    public string IconPath { get; set; } = null!;
    public string TagBackgroundColor { get; set; } = null!;
    public ServiceType ServiceType { get; set; }
    public ICollection<AssetResponseDto> Images { get; set; }
}