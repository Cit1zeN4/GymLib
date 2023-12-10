using GymLibAPI.Models.Product.Request;

namespace GymLibAPI.Models.Food.Request;

public class FoodListRequest : SkipTakeParam
{
    public string? Search { get; set; }
    public DateTimeOffset? DateStart { get; set; }
    public DateTimeOffset? DateEnd { get; set; }
    public FoodOrderType OrderBy { get; set; }
    public OrderDirectionType OrderDirection { get; set; }
}