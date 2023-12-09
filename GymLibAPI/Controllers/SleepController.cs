using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models;
using GymLibAPI.Models.Sleep;
using GymLibAPI.Models.Sleep.Request;
using GymLibAPI.Models.Sleep.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SleepController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<ResponseData<SleepResponse>>> GetByDate(DateTimeOffset dateStart,
        DateTimeOffset dateEnd, int skip = 0, int take = 0)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var query = context.Sleep
            .Where(x => x.Date >= dateStart && x.Date < dateEnd && x.UserId == userId);
        var totalCount = await query.CountAsync();
        query = query.Skip(skip);
        if (take > 0)
            query = query.Take(take);

        var list = await query.Select(x => new SleepResponse
        {
            Id = x.Id,
            Value = x.Value,
            Date = x.Date
        }).ToListAsync();
        
        var response = new ResponseData<SleepResponse>
        {
            TotalCount = totalCount,
            Records = list,
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SleepResponse>> AddSleepData([FromBody] SleepRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var sleep = new SleepEntity
        {
            UserId = userId,
            Value = request.Value,
            Date = request.Date
        };

        context.Sleep.Add(sleep);
        await context.SaveChangesAsync();
        var response = new SleepResponse
        {
            Id = sleep.Id,
            Value = sleep.Value,
            Date = sleep.Date
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateSleepData(int id, [FromBody] SleepUpdateRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var sleep = await context.Sleep.FirstOrDefaultAsync(x => x.Id == id);

        if (sleep == null)
            return NotFound($"Данные сна с Id {id} не найдены");
        if (sleep.UserId != userId)
            return BadRequest("Вы можете редактировать только свои данные");

        if (request.Value.HasValue)
            sleep.Value = request.Value.Value;
        if (request.Date.HasValue)
            sleep.Date = request.Date.Value;

        await context.SaveChangesAsync();
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteSleepData(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var sleep = await context.Sleep.FirstOrDefaultAsync(x => x.Id == id);

        if (sleep == null)
            return NotFound($"Данные сна с Id {id} не найдены");
        if (sleep.UserId != userId)
            return BadRequest("Вы можете удалять только свои данные");

        context.Sleep.Remove(sleep);
        await context.SaveChangesAsync();
        
        return Ok();
    }
}