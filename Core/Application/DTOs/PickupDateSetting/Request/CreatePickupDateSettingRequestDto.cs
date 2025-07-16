namespace Yu.Application.DTOs;

public class CreatePickupDateSettingRequestDto
{
    // Sunday = 0, Monday = 1, ..., Saturday = 6
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}