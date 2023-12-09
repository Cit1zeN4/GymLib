using System.Security.Claims;

namespace GymLibAPI.Infrastructure;

public static class ClaimsPrincipalExtension
{
    public static int GetUserId(this ClaimsPrincipal principal)
    {
        var value = principal.FindFirst(ClaimTypes.NameIdentifier).Value;
        var result = Convert.ToInt32(value);
        return result;
    }
}