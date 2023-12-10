using GymLibAPI.Models.Food;

namespace GymLibAPI.Models.Product;

public class ProductEntity
{
    public ProductEntity()
    {
        
    }
    
    public ProductEntity(Product entity)
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
    public List<FoodEntity> Foods { get; set; }
}