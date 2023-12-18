using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace API.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsWithAddress(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return await userManager.Users.Include(u => u.Address).SingleOrDefaultAsync(u => u.Email == email); //return user with his address
        }

        public static async Task<AppUser> FindByEmailFromClaims(this UserManager<AppUser> userManager, ClaimsPrincipal user)
        {
            return await userManager.Users.SingleOrDefaultAsync(u => u.Email == user.FindFirstValue(ClaimTypes.Email));
        }
    }
}
