using System.Security.Claims;
using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models;
using GymLibAPI.Models.Exercise;
using GymLibAPI.Models.Exercise.Request;
using GymLibAPI.Models.User;
using GymLibAPI.Models.User.Dto;
using GymLibAPI.Models.User.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [HttpPost("list")]
    public async Task<ActionResult> GetUserList([FromBody] UserListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var query = context.Users.Select(x => new UserShortDto(x));

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.UserName.Contains(request.Search));

        var totalCount = await query.CountAsync();

        query = request.OrderBy switch
        {
            UserOrderByType.Id => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Id)
                : query.OrderByDescending(x => x.Id),
            UserOrderByType.Name => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.UserName)
                : query.OrderByDescending(x => x.UserName),
            _ => query
        };

        query = query.Skip(request.Skip);
        if (request.Take > 0)
            query = query.Take(request.Take);

        var list = await query.ToListAsync();
        var response = new ResponseData<UserShortDto>
        {
            TotalCount = totalCount,
            Records = list
        };

        return Ok(response);
    }
    
    [Authorize]
    [HttpPost("follow")]
    public async Task<ActionResult<string>> Follow(int userId)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var followerId = User.GetUserId();
        if (followerId == userId)
            return BadRequest("Вы не можете подписаться на себя");
        
        var user = await context.Users.Include(x => x.Followers).FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            return NotFound($"Пользователь с Id: {userId} не найден");
        
        if (user.Followers.Any(x => x.Id == followerId))
            return BadRequest("Вы уже подписаны на этого пользователя");
        var follower = await context.Users.FirstOrDefaultAsync(x => x.Id == followerId);
        user.Followers.Add(follower);
        
        await context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpPost("unfollow")]
    public async Task<ActionResult<string>> Unfollow(int userId)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        
        var followerId = User.GetUserId();
        if (followerId == userId)
            return BadRequest("Вы не можете отписаться от себя");

        var user = await context.Users.Include(x => x.Followers).FirstOrDefaultAsync(x => x.Id == userId);
        if (user == null)
            return NotFound($"Пользователь с Id: {userId} не найден");
        
        if (!user.Followers.Any(x => x.Id == followerId))
            return BadRequest("Вы не подписаны на этого пользователя");
        var follower = await context.Users.FirstOrDefaultAsync(x => x.Id == followerId);
        user.Followers.Remove(follower);
        
        await context.SaveChangesAsync();
        return Ok();
    }
}