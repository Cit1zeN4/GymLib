using GymLibAPI.Models.User;

namespace GymLibAPI.Models.Article;

public class ArticleEntity
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public int UserId { get; set; }
    public UserEntity User { get; set; }
    public int Views { get; set; }
}