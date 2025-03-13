using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Ticketing.QueryAPI.Models
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public int MssqlId { get; set; }  // MSSQL 產生的 ID
        public int TicketId {  get; set; }
        public int UserId {  get; set; }
        public string Status {  get; set; }
        public DateTime CreatedAt {  get; set; }
    }
}
