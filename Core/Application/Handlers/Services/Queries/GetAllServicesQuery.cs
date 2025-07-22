namespace Yu.Application.Handlers;

public record GetAllServicesQuery : IRequest<List<ServiceResponseDto>>;

internal class GetAllServicesQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetAllServicesQuery, List<ServiceResponseDto>>
{
    public async Task<List<ServiceResponseDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Services
            .Include(s => s.Images)
            .Include(s => s.Icon)
            .Select(s => new ServiceResponseDto
            {
                Id = s.Id,
                Title = s.Title,
                TagTextColor = s.TagTextColor,
                IconPath = s.Icon!.Path,
                TagBackgroundColor = s.TagBackgroundColor,
                SubTitle = s.SubTitle,
                Tag = s.Tag,
                ServiceType = s.ServiceType,
                Images = s.Images.Select(i => new AssetResponseDto
                {
                    FileName = i.Name,
                    Path = i.Path,
                    AssetId = i.Id
                }).ToList()
            })
            .ToListAsync(cancellationToken);
    }
}