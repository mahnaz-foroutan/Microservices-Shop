using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.Core.Entities
{
    [BsonCollection("ProductBrand")]
    public class ProductBrand :BaseEntity
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }
}