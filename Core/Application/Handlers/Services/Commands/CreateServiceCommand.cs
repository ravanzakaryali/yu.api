namespace Yu.Application.Handlers;

public record CreateServiceCommand(string Title, string SubTitle, string Desciption, List<int> ImagesIds) :
    IRequest<ServiceResponseDto>;

public class CreateServiceCommandHandler(IYuDbContext dbContext) : IRequestHandler<CreateServiceCommand, ServiceResponseDto>
{
    public async Task<ServiceResponseDto> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        List<Domain.Entities.File> files = await dbContext.Files
            .ToListAsync(cancellationToken);

        files = [.. files.Where(x => request.ImagesIds.Contains(x.Id))];

        EntityEntry<Service> serviceEntry = dbContext.Services.Add(new Service
        {
            Title = request.Title,
            SubTitle = request.SubTitle,
            Desciption = request.Desciption,
            Images = files
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResponseDto
        {
            Id = serviceEntry.Entity.Id,
            Title = request.Title,
            SubTitle = request.SubTitle,
            Desciption = request.Desciption,
            Images = [.. files.Select(x => new AssetResponseDto
            {
                FileName = x.Name,
                Path = x.Path,
                AssetId = x.Id
            })]
        };
    }
};