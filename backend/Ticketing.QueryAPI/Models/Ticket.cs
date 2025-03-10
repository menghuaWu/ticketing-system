using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ticketing.QueryAPI.Models
{
    public class TicketDTO
    {
        public int Id { get; set; }

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
    public class Ticket
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int MssqlId { get; set; }  // MSSQL 產生的 ID

        [BsonElement("title")]
        public string Title { get; set; } = string.Empty;

        [BsonElement("price")]
        public decimal Price { get; set; }
    }
}
