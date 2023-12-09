using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GymLibAPI.Models.User;

public class UserEntity : IdentityUser<int>
{
    public List<UserEntity> Following { get; set; }
    public List<UserEntity> Followers { get; set; }
}