using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Entities;
using Ordering.Infrastructure.Persistence;

namespace Ordering.Infrastructure.Repositories
{
    public class OrderRepository : RepositoryBase<Order>, IOrderRepository
    {
        public OrderRepository(OrderContext dbContext) : base(dbContext) { }


        //public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        //{
        //    throw new NotImplementedException();
        //}

        public async Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            return await _dbContext.Orders
               .Where(s => s.BuyerEmail == buyerEmail && s.Id == id).Include(s => s.DeliveryMethod)
                        .Include(s => s.OrderItems).Include(s => s.ShipToAddress)
               .FirstOrDefaultAsync();
        }


        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            return await _dbContext.Orders
               .Where(s => s.BuyerEmail == buyerEmail).Include(s => s.DeliveryMethod)
                        .Include(s => s.OrderItems).Include(s => s.ShipToAddress)
               .ToListAsync();
        }

        public async Task<Order> GetOrdersForPaymentIdAsync(string paymentIntentId)
        {
            return await _dbContext.Orders
               .Where(s => s.PaymentIntentId == paymentIntentId).Include(s => s.DeliveryMethod)
                        .Include(s => s.OrderItems).Include(s => s.ShipToAddress)
               .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _dbContext.DeliveryMethods
            .ToListAsync();
        }
        public async Task<DeliveryMethod> GetDeliveryMethodsById(int id)
        {
            return await _dbContext.DeliveryMethods.FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
