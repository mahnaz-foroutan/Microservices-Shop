using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Infrastructure.Data
{
    public class ProductDatabaseSettings: IProductDatabaseSettings
    {
        public string? ProductsCollectionName { get; set; }
        public string? ProductBrandsCollectionName { get; set; }
        public string? ProductTypesCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }

    public interface IProductDatabaseSettings
    {
        public string? ProductsCollectionName { get; set; }
        public string? ProductBrandsCollectionName { get; set; }
        public string? ProductTypesCollectionName { get; set; }
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }
    }
}

