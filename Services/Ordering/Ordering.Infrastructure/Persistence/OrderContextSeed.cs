using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;

namespace Ordering.Infrastructure.Persistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext)
        {
            //if (!await orderContext.Orders.AnyAsync())
            //{
            //   // await orderContext.Orders.AddRangeAsync(GetPreconfiguredOrders());
            //    await orderContext.SaveChangesAsync();
            //    logger.LogInformation("data seed section configured");
            //}
        }

        //public static IEnumerable<Order> GetPreconfiguredOrders()
        //{
        //    return new List<Order>
        //    {
        //        new Order
        //        {
        //            FirstName = "mahnaz",
        //            LastName = "foroutan",
        //            UserName = "mahnaz",
        //            EmailAddress = "test@test.com",
        //            City = "mashhad",
        //            Country = "iran",
        //            TotalPrice = 10000
        //        }
        //    };
        //}
    }
}
