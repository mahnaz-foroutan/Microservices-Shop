
using Microsoft.AspNetCore.Identity;
using Ordering.Domain.Entities.Identity;

namespace Ordering.Infrastructure.Persistence.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Mahnaz",
                    Email = "Mahnaz@test.com",
                    UserName = "Mahnaz@test.com",
                    Address = new Address
                    {
                        FirstName = "Mahnaz",
                        LastName = "F",
                        Street = "10 The street",
                        City = "Mashhad",
                        State = "HSM",
                        Zipcode = "902104"
                    }
                };

                await userManager.CreateAsync(user, "Pa$$w0rd");
            }
        }
    }
}