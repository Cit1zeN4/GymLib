using GymLibAPI.Business.Services.Interfaces;

namespace GymLibAPI.Business.Services;

public class FoodDataService : IFoodDataService
{
    private object _locaker = new();
    private int _interval = 216;
}