using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Core.Entities
{
    [BsonCollection("Product")]
    public class Product:BaseEntity
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? PictureUrl { get; set; }

        public decimal Price { get; set; }

        //public ProductType ProductType { get; set; }
        public string? ProductTypeId { get; set; }
        //public ProductBrand ProductBrand { get; set; }
        public string? ProductBrandId { get; set; }
    }
}
