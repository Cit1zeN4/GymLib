namespace GymLibAPI.Models.Exercise.Request;

public class ExerciseListRequest : SkipTakeParam
{
    public string? Search { get; set; }
    public ExerciseSearchByType SearchBy { get; set; }
    public ExerciseOrderByType  OrderBy { get; set; }
    public OrderDirectionType OrderDirection { get; set; }
}