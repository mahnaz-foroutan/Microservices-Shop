using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.Infrastructure.Data;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using MongoDB.Bson;
using MongoDB.Driver;
using Catalog.Core.Interfaces;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        #region constructor

        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext catalogContext)
        {
            _context = catalogContext;
        }

        #endregion

        #region product repo
        private IQueryable<Product> ApplySpecificationProduct(ProductSpecParams productParams)
        {
            return SpecificationEvaluatorProduct.GetQuery(_context.Products.AsQueryable(), productParams);
        }

        public  int? Count(ProductSpecParams productParams)
        {
            return ApplySpecificationProduct(productParams).Count();
        }

        public IReadOnlyList<Product> ListAsync(ProductSpecParams productParams)
        {
            var query = ApplySpecificationProduct(productParams);

            return  query.ToList();
        }
        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.Products
              .Find(p => p.Id == id && p.ProductBrandId.Length>0 && p.ProductTypeId.Length>0).FirstOrDefaultAsync();
        }

        //public async Task<ProductToReturnDto> GetProductByIdAsync(string id)
        //{
        //    var product= await _context.Products
        //      .Find(p => p.Id == id && p.ProductBrandId.Length > 0 && p.ProductTypeId.Length > 0).FirstOrDefaultAsync();
        //    if(product!=null)
        //    {

        //    }
        //}

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            return await _context.Products.Find(p => true && p.ProductBrandId.Length > 0 && p.ProductTypeId.Length > 0).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.Find(p => true ).ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.Find(p => true).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter =
                Builders<Product>.Filter.Eq(p => p.Name, name);

            return await _context.Products.Find(filter).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await _context.Products.InsertOneAsync(product);
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await _context.Products
                .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);


            return updateResult.IsAcknowledged
                   && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteProduct(string id)
        {
            FilterDefinition<Product> filter =
                Builders<Product>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context.Products
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAll()
        {

            DeleteResult deleteResult = await _context.Products.DeleteManyAsync(Builders<Product>.Filter.Empty);

            return deleteResult.IsAcknowledged
                   && deleteResult.DeletedCount > 0;
        }

        #endregion
    }
}
