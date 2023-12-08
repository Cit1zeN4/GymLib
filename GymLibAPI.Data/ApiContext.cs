using GymLibAPI.Models.Article;
using GymLibAPI.Models.Exercise;
using GymLibAPI.Models.Product;
using GymLibAPI.Models.Role;
using GymLibAPI.Models.Sleep;
using GymLibAPI.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Data;

public class ApiContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options)
    {
        Database.Migrate();
    }
    
    public DbSet<ExerciseEntity> Exercise { get; set; }
    public DbSet<SleepEntity> Sleep { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<ArticleEntity> Articles { get; set; }
}