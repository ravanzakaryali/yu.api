namespace Yu.Application.Handlers;

public record GetAllPickupDateQuery : IRequest<List<GetPickupDateSettingResponseDto>>;

public class GetAllPickupDateQueryHandler(IYuDbContext yuDbContext) : IRequestHandler<GetAllPickupDateQuery, List<GetPickupDateSettingResponseDto>>
{
    public async Task<List<GetPickupDateSettingResponseDto>> Handle(GetAllPickupDateQuery request, CancellationToken cancellationToken)
    {
        return await yuDbContext.PickupDateSettings
            .Select(p => new GetPickupDateSettingResponseDto
            {
                Id = p.Id,
                DayOfWeek = p.DayOfWeek,
                StartTime = p.StartTime,
                EndTime = p.EndTime
            })
            .ToListAsync(cancellationToken);
    }
}