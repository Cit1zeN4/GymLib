using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models;
using GymLibAPI.Models.Food.Dto;
using GymLibAPI.Models.Stat;
using GymLibAPI.Models.Stat.Food.Dto;
using GymLibAPI.Models.Stat.Sleep.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class StatController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [Authorize]
    [HttpGet("food")]
    public async Task<ActionResult<FoodStat>> GetFoodStat()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var now = DateTimeOffset.Now;
        var toDay = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero);
        var toDayEnd = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, TimeSpan.Zero);
        var lastWeek = toDay.AddDays(-7);

        var toDayData = await context.Food
            .Where(x => x.Date >= toDay && x.Date <= toDayEnd && x.UserId == userId)
            .Select(x => new FoodShortDto
            {
                Id = x.Id,
                Name = x.Name,
                Date = x.Date,
                Proteins = x.Products.Sum(y => y.Product.Proteins),
                Fats = x.Products.Sum(y => y.Product.Fats),
                Carbohydrates = x.Products.Sum(y => y.Product.Carbohydrates),
                Kcal = x.Products.Sum(y => y.Product.Kcal)
            }).ToListAsync();

        var toDayFood = new FoodDayStatDto
        {
            Date = toDay,
            ToDayList = toDayData
        };

        var toDayValue = new NutritionalValue
        {
            Proteins = toDayData.Sum(x => x.Proteins),
            Fats = toDayData.Sum(x => x.Fats),
            Carbohydrates = toDayData.Sum(x => x.Carbohydrates),
            Kcal = toDayData.Sum(x => x.Kcal)
        };

        var lastWeekFoods =
            (await context.Food
                .Where(x => x.Date >= lastWeek && x.Date < toDay && x.UserId == userId)
                .Select(x => new FoodDayStatDto
                {
                    Date = new DateTimeOffset(x.Date.Year, x.Date.Month, x.Date.Day, 0, 0, 0, TimeSpan.Zero),
                    ToDayList = new List<FoodShortDto>
                    {
                        new FoodShortDto
                        {
                            Id = x.Id,
                            Name = x.Name,
                            Date = x.Date,
                            Proteins = x.Products.Sum(y => y.Product.Proteins),
                            Fats = x.Products.Sum(y => y.Product.Fats),
                            Carbohydrates = x.Products.Sum(y => y.Product.Carbohydrates),
                            Kcal = x.Products.Sum(y => y.Product.Kcal)
                        }
                    }
                })
                .ToListAsync())
            .GroupBy(x => x.Date)
            .Select(x => new FoodDayStatDto
            {
                Date = x.Key,
                ToDayList = x.SelectMany(y => y.ToDayList, (parent, child) => child).ToList()
            })
            .OrderBy(x => x.Date)
            .ToList();

        var lastWeekValue = new NutritionalValue
        {
            Proteins = lastWeekFoods.Sum(x => x.ToDayList.Sum(y => y.Proteins)),
            Fats = lastWeekFoods.Sum(x => x.ToDayList.Sum(y => y.Fats)),
            Carbohydrates = lastWeekFoods.Sum(x => x.ToDayList.Sum(y => y.Carbohydrates)),
            Kcal = lastWeekFoods.Sum(x => x.ToDayList.Sum(y => y.Kcal)),
        };

        var response = new FoodStat
        {
            ToDay = toDayValue,
            ToDayLits = toDayFood,
            LastWeek = lastWeekValue,
            LastWeekList = lastWeekFoods
        };

        return Ok(response);
    }

    [Authorize]
    [HttpGet("sleep")]
    public async Task<ActionResult<SleepStat>> GetSleepStat()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var now = DateTimeOffset.Now;
        var toDay = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, TimeSpan.Zero);
        var toDayEnd = new DateTimeOffset(now.Year, now.Month, now.Day, 23, 59, 59, TimeSpan.Zero);
        var lastWeek = toDay.AddDays(-7);

        var toDaySleep = await context.Sleep
            .Where(x => x.Date >= toDay && x.Date <= toDayEnd && x.UserId == userId)
            .SumAsync(x => x.Value);

        var lastWeekSleep =
            (await context.Sleep
                .Where(x => x.Date >= lastWeek && x.Date < toDay && x.UserId == userId)
                .Select(x => new
                {
                    Date = new DateTimeOffset(x.Date.Year, x.Date.Month, x.Date.Day, 0, 0, 0, TimeSpan.Zero),
                    Value = x.Value
                })
                .ToListAsync())
            .GroupBy(x => x.Date)
            .Select(x => new SleepDayStatDto
            {
                Date = x.Key,
                Value = x.Sum(y => y.Value),
            })
            .OrderBy(x => x.Date)
            .ToList();

        var response = new SleepStat
        {
            ToDay = toDaySleep,
            LastWeek = lastWeekSleep
        };

        return Ok(response);
    }
}