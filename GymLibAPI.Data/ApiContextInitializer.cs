using Microsoft.Extensions.DependencyInjection;

namespace GymLibAPI.Data;

public static class ApiContextInitializer 
{
    public static void Init(IServiceProvider provider)
    {
        using var context = provider.GetRequiredService<ApiContext>();
    }
}