using GymLibAPI.Models.Role;
using GymLibAPI.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GymLibAPI.Data;

public class ApiContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public ApiContext(DbContextOptions<ApiContext> options) : base(options) { }
}