namespace Yu.Application.Handlers;

public record DeleteAssetCommand(int AssetId) : IRequest;

internal class DeleteAssetCommandHandler(IYuDbContext context, IStorageService storageService) : IRequestHandler<DeleteAssetCommand>
{
    public async Task Handle(DeleteAssetCommand request, CancellationToken cancellationToken)
    {
        Domain.Entities.File? file = await context.Files
            .FirstOrDefaultAsync(f => f.Id == request.AssetId, cancellationToken)
            ?? throw new NotFoundException(nameof(Domain.Entities.File), request.AssetId);

        storageService.Delete(file.Name, "assets");

        context.Files.Remove(file);

        await context.SaveChangesAsync(cancellationToken);
    }
}