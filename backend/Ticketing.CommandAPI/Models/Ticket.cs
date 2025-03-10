using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ticketing.CommandAPI.Models
{
    public class TicketDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
    public class Ticket
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
