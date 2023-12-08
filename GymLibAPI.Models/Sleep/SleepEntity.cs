using GymLibAPI.Models.User;

namespace GymLibAPI.Models.Sleep;

public class SleepEntity
{
    public int Id { get; set; }
    public float Value { get; set; }
    public DateTimeOffset Date { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
}