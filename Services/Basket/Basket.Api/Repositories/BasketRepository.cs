using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.Api.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDistributedCache _redisCache;

        public BasketRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache;
        }

        public async Task<ShoppingCart> GetBasketAsync(string basketId)
        {
            var basket =await  _redisCache.GetStringAsync(basketId);

            return String.IsNullOrEmpty(basket) ? null : JsonConvert.DeserializeObject<ShoppingCart>(basket);
        }

        public async Task<ShoppingCart> UpdateBasketAsync(ShoppingCart basket)
        {
            await _redisCache.SetStringAsync(basket.Id, JsonConvert.SerializeObject(basket));

            

            return await GetBasketAsync(basket.Id);
        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
            await _redisCache.RemoveAsync(basketId);
            return true;
        }

        //public async Task<ShoppingCart> GetUserBasket(string userName)
        //{
        //    var basket = await _redisCache.StringGetAsync(userName);
        //    if (string.IsNullOrEmpty(basket))
        //        return null;

        //    return JsonConvert.DeserializeObject<ShoppingCart>(basket);
        //}
        //public async Task DeleteUserBasket(string userName)
        //{
        //    await _redisCache.KeyDeleteAsync(userName);
        //}

    }
}
