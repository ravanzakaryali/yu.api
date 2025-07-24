namespace Yu.Application.Handlers;

public record GetServiceDetailQuery(int Id) : IRequest<ServiceDetailResponseDto>;

internal class GetServiceDetailQueryHandler(IYuDbContext dbContext) : IRequestHandler<GetServiceDetailQuery, ServiceDetailResponseDto>
{
    public async Task<ServiceDetailResponseDto> Handle(GetServiceDetailQuery request, CancellationToken cancellationToken)
    {
        var service = await dbContext.Services
            .Include(s => s.Images)
            .Include(s => s.Icon)
            .Include(s => s.Price)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken) 
                ?? throw new NotFoundException("Service tapılmadı");


        return new ServiceDetailResponseDto
        {
            Id = service.Id,
            Title = service.Title,
            Tag = service.Tag,
            SubTitle = service.SubTitle,
            TagTextColor = service.TagTextColor,
            TagBackgroundColor = service.TagBackgroundColor,
            IconPath = service.Icon?.Path ?? string.Empty,
            ServiceType = service.ServiceType,
            Images = service.Images.Select(i => new AssetResponseDto
            {
                FileName = i.Name,
                Path = i.Path,
                AssetId = i.Id
            }).ToList(),
            EstimatedDays = service.EstimatedDays,
            Price = service.Price == null ? null : new PriceResponseDto
            {
                Id = service.Price.Id,
                Value = service.Price.Value,
                Currency = service.Price.Currency
            },
           
        };
    }
} 