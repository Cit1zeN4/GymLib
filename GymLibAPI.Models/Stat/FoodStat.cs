using GymLibAPI.Models.Stat.Food.Dto;

namespace GymLibAPI.Models.Stat;

public class FoodStat
{
    public NutritionalValue ToDay { get; set; }
    public FoodDayStatDto ToDayLits { get; set; }
    public NutritionalValue LastWeek { get; set; }
    public List<FoodDayStatDto> LastWeekList { get; set; }
}