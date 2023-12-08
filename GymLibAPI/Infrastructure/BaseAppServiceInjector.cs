using GymLibAPI.Data;
using GymLibAPI.Models.User;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace GymLibAPI.Infrastructure;

public static class BaseAppServiceInjector
{
    public static void Inject(IServiceCollection services)
    {
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddControllers().AddNewtonsoftJson();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        });
        
        services.AddAuthorization();
        services.AddIdentityApiEndpoints<UserEntity>()
            .AddEntityFrameworkStores<ApiContext>();
        services.AddRouting(options => options.LowercaseUrls = true);
    }
}