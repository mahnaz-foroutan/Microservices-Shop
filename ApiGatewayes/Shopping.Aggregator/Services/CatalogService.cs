using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client;
        }

        public async Task<IEnumerable<ProductTBModel>> GetBrands()
        {
            var response = await _client.GetAsync("/api/v1/Catalog/brands");
            return await response.ReadContentAs<List<ProductTBModel>>();
        }

        public async Task<IEnumerable<ProductTBModel>> GetTypes()
        {
            var response = await _client.GetAsync($"/api/v1/Catalog/types");
            return await response.ReadContentAs<List<ProductTBModel>>();
        }

    }
}
