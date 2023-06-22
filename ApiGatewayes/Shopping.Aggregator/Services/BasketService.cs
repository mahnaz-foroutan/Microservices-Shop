using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client;
        }

        public async Task<BasketModel> GetBasketAsync(string basketId)
        {
            var response = await _client.GetAsync($"/api/v1/basket/{basketId}");
            return await response.ReadContentAs<BasketModel>();
        }
    }
}
