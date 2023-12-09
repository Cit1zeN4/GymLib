using GymLibAPI.Models.User;

namespace GymLibAPI.Models.Training;

public class TrainingEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public List<TrainingSetEnitity> Sets { get; set; }
}