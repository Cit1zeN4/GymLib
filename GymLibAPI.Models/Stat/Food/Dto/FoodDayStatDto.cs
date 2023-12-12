using GymLibAPI.Models.Food.Dto;

namespace GymLibAPI.Models.Stat.Food.Dto;

public class FoodDayStatDto
{
    public DateTimeOffset Date { get; set; }
    public List<FoodShortDto> ToDayList { get; set; }
}