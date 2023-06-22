using Basket.Api.Entities;
using System.Threading.Tasks;

namespace Basket.Api.Repositories
{
    public interface IBasketRepository
    {
         Task<ShoppingCart> GetBasketAsync(string basketId); 
        Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket);
        Task<bool> DeleteBasketAsync(string basketId);
    }
}
