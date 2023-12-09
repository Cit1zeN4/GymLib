namespace GymLibAPI.Models.Sleep.Request;

public class SleepUpdateRequest
{
    public float? Value { get; set; }
    public DateTimeOffset? Date { get; set; }
}