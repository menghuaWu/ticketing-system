using System.ComponentModel.DataAnnotations;

namespace Ticketing.CommandAPI.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public int TicketId { get; set; }
        public int UserId {  get; set; }
        public string Status {  get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual Ticket Ticket { get; set; }
    }
}
