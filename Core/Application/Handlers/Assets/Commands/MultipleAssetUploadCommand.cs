using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Yu.Application.Handlers;

public record MultipleAssetUploadCommand(IFormFileCollection Files) : IRequest<MultipleAssetResponseDto>;

internal class MultipleAssetUploadCommandHandler(IStorageService storageService, IYuDbContext context) : IRequestHandler<MultipleAssetUploadCommand, MultipleAssetResponseDto>
{
    public async Task<MultipleAssetResponseDto> Handle(MultipleAssetUploadCommand request, CancellationToken cancellationToken)
    {
        List<FileUploadDto> uploadResults = await storageService.UploadFilesAsync(request.Files, "assets");

        List<Domain.Entities.File> files = new();

        foreach (FileUploadDto uploadResult in uploadResults)
        {
            EntityEntry<Domain.Entities.File> entry = await context.Files.AddAsync(new Domain.Entities.File
            {
                Extension = uploadResult.Extension,
                Name = uploadResult.FileName,
                Path = uploadResult.PathName,
                Size = uploadResult.Size,
            }, cancellationToken);

            files.Add(entry.Entity);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new MultipleAssetResponseDto
        {
            Assets = files.Select(f => new AssetResponseDto
            {
                AssetId = f.Id,
                Path = f.Path ?? string.Empty,
                FileName = f.Name
            }).ToList()
        };
    }
} 