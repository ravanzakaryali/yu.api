
namespace Yu.Application.Handlers;

public record CreatePickupDateCommand(List<CreatePickupDateSettingRequestDto> CreatePickupDates) : IRequest;

public class CreatePickupDateCommandHandler(IYuDbContext yuDbContext) : IRequestHandler<CreatePickupDateCommand>
{
    public async Task Handle(CreatePickupDateCommand request, CancellationToken cancellationToken)
    {
        await yuDbContext.PickupDateSettings.AddRangeAsync(request.CreatePickupDates.Select(p => new PickupDateSetting
        {
            DayOfWeek = p.DayOfWeek,
            StartTime = p.StartTime,
            EndTime = p.EndTime
        }), cancellationToken);
        await yuDbContext.SaveChangesAsync(cancellationToken);
    }
}