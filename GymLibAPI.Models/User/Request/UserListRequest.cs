namespace GymLibAPI.Models.User.Request;

public class UserListRequest : SkipTakeParam
{
    public string? Search { get; set; }
    public UserOrderByType OrderBy { get; set; }
    public OrderDirectionType OrderDirection { get; set; }
}