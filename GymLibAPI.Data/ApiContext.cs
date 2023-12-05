using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Data;

public class ApiContext : DbContext
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
}