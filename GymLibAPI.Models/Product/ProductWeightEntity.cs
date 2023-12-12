namespace GymLibAPI.Models.Product;

public class ProductWeightEntity
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public ProductEntity Product { get; set; }
    public float Weight { get; set; }
}