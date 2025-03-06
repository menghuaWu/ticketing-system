using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ticketing.QueryAPI.Models
{
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
