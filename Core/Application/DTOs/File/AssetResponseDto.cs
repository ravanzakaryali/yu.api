namespace Yu.Application.DTOs;

public class AssetResponseDto
{
    public int AssetId { get; set; }
    public string Path { get; set; } = null!;
    public string FileName { get; set; } = null!;
}