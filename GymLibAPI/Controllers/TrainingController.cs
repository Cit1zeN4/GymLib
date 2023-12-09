using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models.Training.Dto;
using GymLibAPI.Models.Training.Request;
using GymLibAPI.Models.Training.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TrainingController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [Authorize]
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<TrainingFullResponse>> GetTraining(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();
        var training = await context.Trainings.Select(x => new TrainingFullResponse
        {
            Id = x.Id,
            Name = x.Name,
            IsPublic = x.IsPublic,
            AuthorId = x.UserId,
            Sets = x.Sets.Select(y => new TrainingSetFullDto
            {
                Id = y.Id,
                ExerciseId = y.ExerciseId,
                Name = y.Exercise.Name,
                Description = y.Exercise.Description,
                Comment = y.Comment
            }).ToList()
        }).FirstOrDefaultAsync(x => x.Id == id);

        if (training == null)
            return NotFound($"Тернировка с Id: {id} не найдена");
        if (training.AuthorId != userId && !training.IsPublic)
            return BadRequest($"Тренировка не является общедоступной");
        
        return Ok(training);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateTraining([FromBody] TrainingRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        
        var training = request.GetTrainingEntity();
        training.UserId = User.GetUserId();
        context.Trainings.Add(training);
        await context.SaveChangesAsync();
        
        return Ok();
    }

    [Authorize]
    [HttpPut]
    public async Task<ActionResult> UpdateTraining([FromBody] TrainingRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();
        
        var training = request.GetTrainingEntity();
        var entity = await context.Trainings.FirstOrDefaultAsync(x => x.Id == training.Id);
        
        if (entity == null)
            return NotFound($"Тренировка с Id {training.Id} не найдена");
        if (entity.UserId != userId)
            return BadRequest("Только автор тренировки может ее редактировать");

        training.UserId = userId;
        context.Trainings.Update(training);
        await context.SaveChangesAsync();
        
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<ActionResult> DeleteTraining(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var training = await context.Trainings.FirstOrDefaultAsync(x => x.Id == id);
        if(training == null)
            return BadRequest($"Тренировка с Id {training.Id} не найдена");
        if(training.UserId != userId)
            return BadRequest("Только автор тренировки может ее удалить");
        
        context.Remove(training);
        await context.SaveChangesAsync();
        return Ok();
    }
    
    [Authorize]
    [AllowAnonymous]
    [HttpGet("user-trainings")]
    public async Task<ActionResult<TrainingShortResponse>> GetTrainings(int userId, int skip, int take)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var id = User.GetUserId();

        var query = context.Trainings.Where(x => x.UserId == userId);
        if (id != userId)
            query = query.Where(x => x.IsPublic);

        var totalCount = await query.CountAsync();
        
        query = query.Skip(skip);
        if (take > 0)
            query = query.Take(take);
        
        var list = await query.ToListAsync();
        var trainings = list.Select(x => new TrainingSetShortDto
        {
            Id = x.Id,
            Name = x.Name
        }).ToList();

        var response = new TrainingShortResponse
        {
            TotalCount = totalCount,
            Trainings = trainings
        };
        
        return Ok(response);
    }
}