using MongoDB.Driver;
using Catalog.Core.Entities;

namespace Catalog.Infrastructure.Data
{
    public interface ICatalogContext
    {
        IMongoCollection<Product> Products { get; }
        IMongoCollection<ProductBrand> ProductBrands { get; }
        IMongoCollection<ProductType> ProductTypes { get; }
    }
}
