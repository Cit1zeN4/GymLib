using GymLibAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Infrastructure;

public static class DbInjector
{
    public static void Inject(WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<ApiContext>(options =>
        {
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
        });


    }
}