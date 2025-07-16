public class GetPickupDateSettingResponseDto
{
    // Sunday = 0, Monday = 1, ..., Saturday = 6
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
}