using System.ComponentModel.DataAnnotations;

namespace GymLibAPI.Models.Article.Request;

public class ArticleRequest
{
    [Required]
    public string Title { get; set; }
    [Required]
    public string Text { get; set; }
}