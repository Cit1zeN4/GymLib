using GymLibAPI.Models.Training.Dto;

namespace GymLibAPI.Models.Training.Response;

public class TrainingShortResponse
{
    public int TotalCount { get; set; }
    public List<TrainingSetShortDto> Trainings { get; set; }
}