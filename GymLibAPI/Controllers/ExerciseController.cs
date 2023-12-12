using GymLibAPI.Data;
using GymLibAPI.Models;
using GymLibAPI.Models.Exercise;
using GymLibAPI.Models.Exercise.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

public class ExerciseController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [HttpPost("list")]
    public async Task<ActionResult<ResponseData<ExerciseEntity>>> GetExerciseList(
        [FromBody] ExerciseListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var query = context.Exercise.AsQueryable();

        if (request.Search != null)
        {
            query = request.SearchBy switch
            {
                ExerciseSearchByType.Name => query.Where(x => x.Name.Contains(request.Search)),
                ExerciseSearchByType.Tags => query.Where(x => x.Tags.Contains(request.Search)),
                _ => query
            };
        }

        var totalCount = await query.CountAsync();

        query = request.OrderBy switch
        {
            ExerciseOrderByType.Id => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Id)
                : query.OrderByDescending(x => x.Id),
            ExerciseOrderByType.Name => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),
            _ => query
        };

        query = query.Skip(request.Skip);
        if (request.Take > 0)
            query = query.Take(request.Take);

        var list = await query.ToListAsync();
        var response = new ResponseData<ExerciseEntity>
        {
            TotalCount = totalCount,
            Records = list
        };

        return Ok(response);
    }

    [HttpGet("/{id:int}")]
    public async Task<ActionResult<ExerciseEntity>> GetExercise(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var exercise = await context.Exercise.FirstOrDefaultAsync(x => x.Id == id);
        if (exercise == null)
            return NotFound($"Упражнение с Id {id} не найдено");

        return Ok(exercise);
    }

    [HttpGet("/{slug}")]
    public async Task<ActionResult<ExerciseEntity>> GetExercise(string slug)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var exercise = await context.Exercise.FirstOrDefaultAsync(x => x.Slug == slug);
        if (exercise == null)
            return NotFound($"Упражнение с Slug {slug} не найдено");

        return Ok(exercise);
    }

    [HttpGet("tags")]
    public async Task<ActionResult<List<string>>> GetAvailableTags()
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var result = await context.Exercise
            .Select(x => x.Tags)
            .Distinct()
            .OrderBy(x => x)
            .ToListAsync();

        var tags = string
            .Join(',', result).Split(',')
            .Distinct().OrderBy(x => x).ToList();

        return Ok(tags);
    }
}