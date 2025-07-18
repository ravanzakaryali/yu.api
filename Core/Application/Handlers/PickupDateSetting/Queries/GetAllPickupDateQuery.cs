namespace Yu.Application.Handlers;

public record GetAllPickupDateQuery : IRequest<List<PickupDateSettingResponseDto>>;

public class GetAllPickupDateQueryHandler(IYuDbContext yuDbContext) : IRequestHandler<GetAllPickupDateQuery, List<PickupDateSettingResponseDto>>
{
    public async Task<List<PickupDateSettingResponseDto>> Handle(GetAllPickupDateQuery request, CancellationToken cancellationToken)
    {
        return await yuDbContext.PickupDateSettings
            .Select(p => new PickupDateSettingResponseDto
            {
                Id = p.Id,
                DayOfWeek = p.DayOfWeek,
                StartTime = p.StartTime,
                EndTime = p.EndTime
            })
            .ToListAsync(cancellationToken);
    }
}