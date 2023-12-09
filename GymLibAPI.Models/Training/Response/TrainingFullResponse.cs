using GymLibAPI.Models.Training.Dto;

namespace GymLibAPI.Models.Training.Response;

public class TrainingFullResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsPublic { get; set; }
    public int AuthorId { get; set; }
    public List<TrainingSetFullDto> Sets { get; set; }
}