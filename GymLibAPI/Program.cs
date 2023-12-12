using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models.User;

var builder = WebApplication.CreateBuilder(args);

DbInjector.Inject(builder);
BaseAppServiceInjector.Inject(builder.Services);
ServiceInjector.Inject(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.MapIdentityApi<UserEntity>();
app.UseHttpsRedirection();
using var scope = app.Services.CreateScope();
ApiContextInitializer.Init(scope.ServiceProvider);
app.Run();

public partial class Program { }