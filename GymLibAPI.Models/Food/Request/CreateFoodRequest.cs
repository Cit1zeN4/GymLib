namespace GymLibAPI.Models.Food.Request;

public class CreateFoodRequest
{
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public List<int> Products { get; set; }
}