using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using GymLibAPI.Models.Exercise;
using GymLibAPI.Models.Product;
using GymLibAPI.Models.Role;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace GymLibAPI.Data;

public static class ApiContextInitializer
{
    public static void Init(IServiceProvider provider)
    {
        using var context = provider.GetRequiredService<ApiContext>();
        InitRoles(context);
        InitExercises(context);
        InitProducts(context);
    }

    private static void InitRoles(ApiContext context)
    {
        if (!context.Roles.Any())
        {
            var roles = new List<RoleEntity>()
            {
                new()
                {
                    Id = 1,
                    Name = "user",
                    NormalizedName = "User"
                },
                new()
                {
                    Id = 2,
                    Name = "admin",
                    NormalizedName = "Admin"
                }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();
        }
    }

    private static void InitExercises(ApiContext context)
    {
        if (!context.Exercise.Any())
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = buildDir + @"\SeedData\exercises.csv";
            using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
            {
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Encoding = Encoding.UTF8,
                    Delimiter = ","
                };
                
                CsvReader csvReader = new CsvReader(reader, config);
                var exercises = csvReader.GetRecords<ExerciseEntity>().ToArray();
                context.Exercise.AddRange(exercises);
                context.SaveChanges();
            }
        }
    }

    private static void InitProducts(ApiContext context)
    {
        if (!context.Products.Any())
        {
            var buildDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = buildDir + @"\SeedData\products.json";

            var text = File.ReadAllText(path, Encoding.UTF8);
            var products = JsonConvert.DeserializeObject<List<Product>>(text);
            if (products != null)
            {
                context.Products.AddRange(products.Select(x => new ProductEntity(x)));
                context.SaveChanges();
            }
        }
    }
}