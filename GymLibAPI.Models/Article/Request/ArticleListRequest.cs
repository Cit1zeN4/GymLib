namespace GymLibAPI.Models.Article.Request;

public class ArticleListRequest : SkipTakeParam
{
    public string? Search { get; set; }
    public ArticleOrderByType OrderBy { get; set; }
    public OrderDirectionType OrderDirection { get; set; }
}