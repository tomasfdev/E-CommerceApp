using Core.Models.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class IdentitySeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "User",
                    Email = "User@test.com",
                    UserName = "User@test.com",
                    Address = new Address
                    {
                        FirstName = "User",
                        LastName = "Test",
                        City = "Lisbon",
                        Street = "Lourinhã",
                        ZipCode = "2530-123"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}
