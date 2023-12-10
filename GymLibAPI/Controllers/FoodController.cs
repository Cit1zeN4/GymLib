using GymLibAPI.Data;
using GymLibAPI.Models;
using GymLibAPI.Models.Product;
using GymLibAPI.Models.Product.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FoodController(IServiceScopeFactory serviceScopeFactory) : Controller
{
    [HttpPost("product-list")]
    public async Task<ActionResult<ResponseData<ProductEntity>>> GetProducts([FromBody] ProductListRequest request)
    {
        using var scope = serviceScopeFactory.CreateScope();
        await using var context = scope.ServiceProvider.GetRequiredService<ApiContext>();

        var query = context.Products.Select(x => x);

        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.Name.Contains(request.Search));

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
}