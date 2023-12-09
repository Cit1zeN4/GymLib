namespace GymLibAPI.Models.Training.Dto;

public class TrainingSetFullDto
{
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Set { get; set; }
    public int Reps { get; set; }
    public string Comment { get; set; }
}