using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace GymLibAPI.Models.User;

public class UserEntity : IdentityUser<int>
{
    
}