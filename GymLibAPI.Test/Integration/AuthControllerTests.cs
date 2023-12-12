using System.Net;
using System.Net.Http.Json;
using GymLibAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GymLibAPI.Test.Integration;

public class AuthControllerTests
{
    private WebApplicationFactory<Program> _application = null!;
    private ApiContext _context = null!;

    private readonly string _email = "user@gmail.com";
    private readonly string _password = "Abc_12345678";

    [SetUp]
    public void SetUp()
    {
        _application = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
        {
            builder.ConfigureTestServices(services =>
            {
                var dbContextDescriptor =
                    services.Single(x => x.ServiceType == typeof(DbContextOptions<ApiContext>));

                services.Remove(dbContextDescriptor);

                services.AddDbContext<ApiContext>(options =>
                {
                    options.UseInMemoryDatabase("gymLib");
                });
            });
        });
        
        _context = _application.Services.CreateScope().ServiceProvider.GetRequiredService<ApiContext>();
    }

    [Test, Order(1)]
    public async Task Register_SendRequest_ShouldReturnOk()
    {
        using var client = _application.CreateClient();
        var response = await client.PostAsJsonAsync("/register", new { Email = _email, Password = _password });
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [Test, Order(2)]
    public async Task User_CheckDb_ShouldExist()
    {
        var isExist = await _context.Users.AnyAsync(x => x.Email == _email);
        Assert.IsTrue(isExist);
    }
    
    [Test, Order(3)]
    public async Task Login_SendRequest_ShouldReturnBadRequest()
    {
        using var client = _application.CreateClient();
        var response = await client.PostAsJsonAsync("/login", new { Email = "invalid" + _email, Password = _password });
        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Test, Order(4)]
    public async Task Login_SendRequest_ShouldReturnOk()
    {
        using var client = _application.CreateClient();
        var response = await client.PostAsJsonAsync("/login", new { Email = _email, Password = _password });
        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
    }

    [TearDown]
    public void TearDown()
    {
        _application.Dispose();
        _context.Dispose();
    }
}