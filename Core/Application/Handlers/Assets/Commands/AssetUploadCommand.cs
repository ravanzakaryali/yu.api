using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Yu.Application.Handlers;

public record AssetUploadCommand(IFormFile File) : IRequest<AssetResponseDto>;

internal class AssetUploadCommandHandler(IStorageService storageService, IYuDbContext context) : IRequestHandler<AssetUploadCommand, AssetResponseDto>
{
    public async Task<AssetResponseDto> Handle(AssetUploadCommand request, CancellationToken cancellationToken)
    {
        FileUploadDto uploadResult = await storageService.UploadFileAsync(request.File, "assets");

        EntityEntry<Domain.Entities.File> entry = await context.Files.AddAsync(new Domain.Entities.File
        {
            Extension = uploadResult.Extension,
            Name = uploadResult.FileName,
            Path = uploadResult.PathName,
            Size = uploadResult.Size,
        });

        await context.SaveChangesAsync(cancellationToken);

        Domain.Entities.File newFile = entry.Entity;

        return new AssetResponseDto
        {
            AssetId = newFile.Id,
            Path = newFile.Path ?? String.Empty,
            FileName = uploadResult.FileName
        };
    }
}