using Shopping.Aggregator.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shopping.Aggregator.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<ProductTBModel>> GetBrands();
        Task<IEnumerable<ProductTBModel>> GetTypes();
    }
}
