using System.Linq;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Catalog.Infrastructure.Data
{
    public class SpecificationEvaluatorProduct
    {
        public static IQueryable<Product> GetQuery(IQueryable<Product> inputQuery, ProductSpecParams productParams)
        {
            var query = inputQuery;
            if(!string.IsNullOrEmpty(productParams.Search))
            {
                query = query.Where(p=> p.Name.ToLower().Contains(productParams.Search));
            }
            if (!string.IsNullOrEmpty(productParams.BrandId))
            {
                query = query.Where(p => p.ProductBrandId == productParams.BrandId);
            }
            if (!string.IsNullOrEmpty(productParams.TypeId))
            {
                query = query.Where(p => p.ProductTypeId == productParams.TypeId);
            }

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        query.OrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        query.OrderByDescending(p => p.Price);
                        break;
                    default:
                        query.OrderBy(n => n.Name);
                        break;
                }
            }

         
            if (productParams.PageSize>0)
            {
                query = query.Skip(productParams.PageSize * (productParams.PageIndex - 1)).Take(productParams.PageSize);
            }
            //if (spec.Includes.Count>0)
            //{
            //    query = spec.Includes.Aggregate(query, (current, include) => current.Where(c => c.Equals(include)));
            //}
            return query;
        }

       
    }
}