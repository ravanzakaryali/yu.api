namespace Yu.Application.Handlers;

public record CreateServiceCommand(string Title, string Tag, string SubTitle,string TagTextColor, string TagBackgroundColor, ServiceType ServiceType, List<int> ImagesIds) :
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
            Tag = request.Tag,
            TagTextColor = request.TagTextColor,
            TagBackgroundColor = request.TagBackgroundColor,
            ServiceType = request.ServiceType,
            SubTitle = request.SubTitle,
            Images = files
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        return new ServiceResponseDto
        {
            Id = serviceEntry.Entity.Id,
            Title = request.Title,
            Tag = request.SubTitle,
            TagTextColor = request.TagTextColor,
            TagBackgroundColor = request.TagBackgroundColor,
            SubTitle = request.SubTitle,
            Images = [.. files.Select(x => new AssetResponseDto
            {
                FileName = x.Name,
                Path = x.Path,
                AssetId = x.Id
            })]
        };
    }
};