using GymLibAPI.Models.Product;
using GymLibAPI.Models.User;

namespace GymLibAPI.Models.Food;

public class FoodEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Date { get; set; }
    public List<ProductEntity> Products { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
}