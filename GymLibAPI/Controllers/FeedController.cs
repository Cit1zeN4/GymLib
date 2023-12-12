using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models;
using GymLibAPI.Models.Article;
using GymLibAPI.Models.Feed.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FeedController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<ResponseData<ArticleEntity>>> GetFeed([FromBody] FeedListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var query = context.Articles.AsQueryable();
        var following = await context.Users
            .SelectMany(x => x.Following, (parent, child) => child.Id)
            .ToListAsync();

        query = query.Where(x => following.Contains(x.UserId));
        
        var totalCount = await query.CountAsync();

        query = query.OrderBy(x => x.CreatedAt).Skip(request.Skip);
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
}