namespace GymLibAPI.Models.Training.Request;

public class TrainingRequest
{
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public List<TrainingSetRequest> Sets { get; set; }

    public TrainingEntity GetTrainingEntity()
    {
        var training = new TrainingEntity();
        training.Name = Name;
        training.IsPublic = IsPublic;
        training.Sets = Sets.Select(x => new TrainingSetEnitity
        {
            Id = x.Id.GetValueOrDefault(),
            ExerciseId = x.ExerciseId,
            Set = x.Set,
            Reps = x.Reps,
            Comment = x.Comment
        }).ToList();

        return training;
    }
}

public abstract class TrainingSetRequest
{
    public int? Id { get; set; }
    public int ExerciseId { get; set; }
    public int Set { get; set; }
    public int Reps { get; set; }
    public string Comment { get; set; }
}