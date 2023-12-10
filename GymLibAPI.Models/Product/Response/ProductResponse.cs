namespace GymLibAPI.Models.Product.Response;

public class ProductResponse
{
    public ProductResponse(ProductEntity? entity = null)
    {
        if (entity != null)
        {
            Id = entity.Id;
            Name = entity.Name;
            Proteins = entity.Proteins;
            Fats = entity.Fats;
            Carbohydrates = entity.Carbohydrates;
            Kcal = entity.Kcal;
        }
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public float Proteins { get; set; }
    public float Fats { get; set; }
    public float Carbohydrates { get; set; }
    public float Kcal { get; set; }
}