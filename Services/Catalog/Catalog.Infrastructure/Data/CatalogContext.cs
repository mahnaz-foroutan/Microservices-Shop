using Catalog.Core.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using static Catalog.Infrastructure.Data.ProductDatabaseSettings;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContext : ICatalogContext
    {
        public IMongoCollection<Product> Products { get; }
        public IMongoCollection<ProductType> ProductTypes { get; }
        public IMongoCollection<ProductBrand> ProductBrands { get; }

        public CatalogContext(IProductDatabaseSettings settings)
        {
            
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            Products = database.GetCollection<Product>(settings.ProductsCollectionName);
            ProductBrands= database.GetCollection<ProductBrand>(settings.ProductBrandsCollectionName);
            ProductTypes= database.GetCollection<ProductType>(settings.ProductTypesCollectionName);
            CatalogContextSeed.SeedData(ProductBrands, ProductTypes,Products);
        }
    }
}
