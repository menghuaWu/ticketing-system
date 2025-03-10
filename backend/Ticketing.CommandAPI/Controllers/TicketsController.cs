using Confluent.Kafka;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using Ticketing.CommandAPI.Data;
using Ticketing.CommandAPI.Models;

namespace Ticketing.CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public TicketsController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(TicketDTO ticketDTO)
        {
            // DTO轉換
            var ticket = new Ticket()
            {
                Title = ticketDTO.Title,
                Price = ticketDTO.Price
            };
            // 寫入MSSQL
            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            // 發送事件到 Kafka
            var kafkaConfig = new ProducerConfig { BootstrapServers = _config["Kafka:BootstrapServers"] };
            using var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
            var ticketEvent = JsonSerializer.Serialize(ticket);
            await producer.ProduceAsync(_config["Kafka:Topic"], new Message<Null, string> { Value = ticketEvent });

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        [HttpGet("{id}")]
        public IActionResult GetTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);
            if (ticket == null) return NotFound();
            return Ok(ticket);
        }
    }
}
