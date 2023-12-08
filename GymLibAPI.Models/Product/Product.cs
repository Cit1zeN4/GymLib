namespace GymLibAPI.Models.Product;

public class Product
{
    public Product(ProductEntity? entity = null)
    {
        if (entity != null)
        {
            Id = entity.Id;
            Name = entity.Name;
            var splited = entity.Bgu
                .Split(",")
                .Select(x => Convert.ToSingle(x))
                .ToArray();
            Proteins = splited[0];
            Fats = splited[1];
            Carbohydrates = splited[2];
            Kcal = entity.Kcal;
        }
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public float Proteins { get; set; }
    public float Fats { get; set; }
    public float Carbohydrates { get; set; }
    public float Kcal { get; set; }

    public ProductEntity GetEntity()
    {
        return new ProductEntity
        {
            Id = Id,
            Name = Name,
            Bgu = $"{Proteins},{Fats},{Carbohydrates}",
            Kcal = Kcal
        };
    }
}