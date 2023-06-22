using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> GetProductByIdAsync(string id);
        Task<IReadOnlyList<Product>> GetProductsAsync();
        Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync();
        Task<IReadOnlyList<ProductType>> GetProductTypesAsync();
        int? Count(ProductSpecParams productParams);
        IReadOnlyList<Product> ListAsync(ProductSpecParams productParams);
        Task<IEnumerable<Product>> GetProductByName(string name);
        Task CreateProduct(Product product);
        Task<bool> UpdateProduct(Product product);
        Task<bool> DeleteProduct(string id);

        Task<bool> DeleteAll();
    }
}
