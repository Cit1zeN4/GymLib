using System.Security.Claims;
using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IServiceScopeFactory serviceScopeFactory) : Controller
{
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