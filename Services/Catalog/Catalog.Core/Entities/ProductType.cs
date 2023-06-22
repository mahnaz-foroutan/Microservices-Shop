using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Catalog.Core.Entities
{
    [BsonCollection("ProductType")]
    public class ProductType :BaseEntity
    {
        //[BsonId]
        //[BsonRepresentation(BsonType.ObjectId)]
        //public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }
}