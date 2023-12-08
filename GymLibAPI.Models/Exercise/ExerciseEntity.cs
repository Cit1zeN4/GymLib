namespace GymLibAPI.Models.Exercise;

public class ExerciseEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Slug { get; set; }
    public string Begginer { get; set; }
    public string Description { get; set; }
    public string Source { get; set; }
    public string Tags { get; set; }
}