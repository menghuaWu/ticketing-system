namespace Ticketing.CommandAPI.Models
{
    public class OrderCreateRequest
    {
        public int TicketId { get; set; }
        public int UserId { get; set; } = 999;
    }
}
