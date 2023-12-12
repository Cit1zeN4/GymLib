using GymLibAPI.Models.Stat.Sleep.Dto;

namespace GymLibAPI.Models.Stat;

public class SleepStat
{
    public float ToDay { get; set; }
    public List<SleepDayStatDto> LastWeek { get; set; }
}