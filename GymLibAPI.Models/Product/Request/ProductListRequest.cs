namespace GymLibAPI.Models.Product.Request;

public class ProductListRequest : SkipTakeParam
{
    public string? Search { get; set; }
    public ProductOrderType OrderBy { get; set; }
    public OrderDirectionType OrderDirection { get; set; }
}