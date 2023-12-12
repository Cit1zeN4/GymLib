using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models;
using GymLibAPI.Models.Article;
using GymLibAPI.Models.Article.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class ArticleController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [HttpPost("list")]
    public async Task<ActionResult<ResponseData<ArticleEntity>>> GetArticleList([FromBody] ArticleListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var query = context.Articles.AsQueryable();

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.Title.Contains(request.Search));

        var totalCount = await query.CountAsync();

        query = request.OrderBy switch
        {
            ArticleOrderByType.Title => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Title)
                : query.OrderByDescending(x => x.Title),
            ArticleOrderByType.CreatedAt => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.CreatedAt)
                : query.OrderByDescending(x => x.CreatedAt),
            _ => query
        };

        query = query.Skip(request.Skip);
        if (request.Take > 0)
            query = query.Take(request.Take);

        var list = await query.ToListAsync();
        var response = new ResponseData<ArticleEntity>
        {
            TotalCount = totalCount,
            Records = list
        };

        return Ok(response);
    }

    [HttpGet]
    public async Task<ActionResult<ArticleEntity>> GetArticle(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var article = await context.Articles.FirstOrDefaultAsync(x => x.Id == id);
        
        if (article == null)
            return NotFound($"Запись с Id {id} не найдена");

        return Ok(article);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ArticleEntity>> CreateArticle([FromBody] ArticleRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var article = new ArticleEntity
        {
            Title = request.Title,
            Text = request.Text,
            CreatedAt = DateTimeOffset.Now,
            UserId = userId
        };

        context.Articles.Add(article);
        await context.SaveChangesAsync();

        return Ok(article);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateArticle(int id, [FromBody] ArticleRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var article = await context.Articles.FirstOrDefaultAsync(x => x.Id == id);
        if (article == null)
            return NotFound($"Запись с Id {id} не найдена");
        if (article.UserId != userId)
            return BadRequest($"Только автор записи может ее редактировть");

        article.Title = request.Title;
        article.Text = request.Text;

        await context.SaveChangesAsync();
        
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteArticle(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();
        
        var article = await context.Articles.FirstOrDefaultAsync(x => x.Id == id);
        if (article == null)
            return NotFound($"Запись с Id {id} не найдена");
        if (article.UserId != userId)
            return BadRequest($"Только автор записи может ее редактировть");

        context.Articles.Remove(article);
        await context.SaveChangesAsync();

        return Ok();
    }
    
}