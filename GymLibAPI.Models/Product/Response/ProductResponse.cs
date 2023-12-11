namespace GymLibAPI.Models.Product.Response;

public class ProductResponse
{
    public ProductResponse(ProductWeightEntity? entity = null)
    {
        if (entity != null)
        {
            Id = entity.Product.Id;
            Name = entity.Product.Name;
            Proteins = entity.Product.Proteins;
            Fats = entity.Product.Fats;
            Carbohydrates = entity.Product.Carbohydrates;
            Kcal = entity.Product.Kcal;
            Weight = entity.Weight;
        }
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public float Proteins { get; set; }
    public float Fats { get; set; }
    public float Carbohydrates { get; set; }
    public float Kcal { get; set; }
    public float Weight { get; set; }
}