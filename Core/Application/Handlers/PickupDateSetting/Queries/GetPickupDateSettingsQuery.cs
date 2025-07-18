namespace Yu.Application.Handlers;

public record GetPickupDateSettingsQuery : IRequest<IEnumerable<PickupDateSettingResponseDto>>;

internal class GetPickupDateSettingsQueryHandler(IYuDbContext yuDbContext) : IRequestHandler<GetPickupDateSettingsQuery, IEnumerable<PickupDateSettingResponseDto>>
{
    public async Task<IEnumerable<PickupDateSettingResponseDto>> Handle(GetPickupDateSettingsQuery request, CancellationToken cancellationToken)
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