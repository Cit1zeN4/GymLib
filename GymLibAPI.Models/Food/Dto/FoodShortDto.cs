namespace GymLibAPI.Models.Food.Dto;

public class FoodShortDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public float Proteins { get; set; }
    public float Fats { get; set; }
    public float Carbohydrates { get; set; }
    public float Kcal { get; set; }
}