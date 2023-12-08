using GymLibAPI.Business.Services;
using GymLibAPI.Business.Services.Interfaces;

namespace GymLibAPI.Infrastructure;

public static class ServiceInjector
{
    public static void Inject(IServiceCollection services)
    {
        services.AddSingleton<IFoodDataService, FoodDataService>();
    }
}