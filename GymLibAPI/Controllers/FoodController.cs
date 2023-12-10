using GymLibAPI.Data;
using GymLibAPI.Infrastructure;
using GymLibAPI.Models;
using GymLibAPI.Models.Food;
using GymLibAPI.Models.Food.Dto;
using GymLibAPI.Models.Food.Request;
using GymLibAPI.Models.Food.Response;
using GymLibAPI.Models.Product;
using GymLibAPI.Models.Product.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [Authorize]
    [HttpGet]
    public async Task<ActionResult<FoodResponse>> GetFood(int id)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();

        var food = await context.Food.FirstOrDefaultAsync(x => x.Id == id);
        
        if (food == null)
            return NotFound($"Данные о питании с Id {id} не найдены");
        if (food.UserId != userId)
            return BadRequest("Данные о питании может получить только владелец");
        
        var response = new FoodResponse(food);
        return Ok(response);
    }
    
    [HttpPost("product-list")]
    public async Task<ActionResult<ResponseData<ProductEntity>>> GetProducts([FromBody] ProductListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var query = context.Products.Select(x => x);

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()));

        var totalCount = await query.CountAsync();

        query = request.OrderBy switch
        {
            ProductOrderType.Id => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Id)
                : query.OrderByDescending(x => x.Id),
            ProductOrderType.Name => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),
            ProductOrderType.Proteins => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Proteins)
                : query.OrderByDescending(x => x.Proteins),
            ProductOrderType.Fats => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Fats)
                : query.OrderByDescending(x => x.Fats),
            ProductOrderType.Carbohydrates => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Carbohydrates)
                : query.OrderByDescending(x => x.Carbohydrates),
            ProductOrderType.Kcal => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Kcal)
                : query.OrderByDescending(x => x.Kcal),
            _ => query
        };

        query = query.Skip(request.Skip);
        if (request.Take > 0)
            query = query.Take(request.Take);

        var list = await query.ToListAsync();
        var response = new ResponseData<ProductEntity>
        {
            TotalCount = totalCount,
            Records = list
        };

        return Ok(response);
    }

    [Authorize]
    [HttpPost("list")]
    public async Task<ActionResult<ResponseData<FoodEntity>>> GetFoodList([FromBody] FoodListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();
        var userId = User.GetUserId();
        
        var query = context.Food.Where(x => x.UserId == userId).Select(x => new FoodShortDto
        {
            Id = x.Id,
            Name = x.Name,
            Date = x.Date,
            Proteins = x.Products.Sum(y => y.Proteins),
            Fats = x.Products.Sum(y => y.Fats),
            Carbohydrates = x.Products.Sum(y => y.Carbohydrates),
            Kcal = x.Products.Sum(y => y.Kcal)
        });
        
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.Name.ToLower().Contains(request.Search.ToLower()));
        if (request.DateStart.HasValue)
            query = query.Where(x => x.Date >= request.DateStart);
        if (request.DateEnd.HasValue)
            query = query.Where(x => x.Date <= request.DateEnd);

        var totalCount = await query.CountAsync();

        query = request.OrderBy switch
        {
            FoodOrderType.Id => request.OrderDirection == OrderDirectionType.ASC 
                ? query.OrderBy(x => x.Id)
                : query.OrderByDescending(x => x.Id),
            FoodOrderType.Name => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Name)
                : query.OrderByDescending(x => x.Name),
            FoodOrderType.Date => request.OrderDirection == OrderDirectionType.ASC
                ? query.OrderBy(x => x.Date)
                : query.OrderByDescending(x => x.Date),
            _ => query
        };
        
        query = query.Skip(request.Skip);
        if (request.Take > 0)
            query = query.Take(request.Take);

        var list = await query.ToListAsync();
        var response = new ResponseData<FoodShortDto>
        {
            TotalCount = totalCount,
            Records = list
        };

        return Ok(response);
    }
}