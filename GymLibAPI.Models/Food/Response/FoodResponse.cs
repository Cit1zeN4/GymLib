using GymLibAPI.Models.Product.Response;

namespace GymLibAPI.Models.Food.Response;

public class FoodResponse
{
    public FoodResponse(FoodEntity? entity = null)
    {
        if (entity != null)
        {
            Id = entity.Id;
            Name = entity.Name;
            Date = entity.Date;
            Products = entity.Products
                .Select(x => new ProductResponse(x))
                .ToList();
        }
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public List<ProductResponse> Products { get; set; }
}