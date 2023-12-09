using GymLibAPI.Models.Exercise;

namespace GymLibAPI.Models.Training;

public class TrainingSetEnitity
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public ExerciseEntity Exercise { get; set; }
    public int Set { get; set; }
    public int Reps { get; set; }
    public string Commnet { get; set; }
}