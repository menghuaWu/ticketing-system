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
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public OrderController(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateRequest request)
        {
            if (request == null || request.TicketId == 0 || request.UserId == 0)
            {
                return BadRequest("Invalid request");
            }

            // 寫入MSSQL
            var order = new Order()
            {
                TicketId = request.TicketId,
                UserId = request.UserId,
                Status = "pedding"
            };
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 發送事件到 Kafka
            var kafkaConfig = new ProducerConfig { BootstrapServers = _config["Kafka:BootstrapServers"] };
            using var producer = new ProducerBuilder<Null, string>(kafkaConfig).Build();
            var ticketOrder = JsonSerializer.Serialize(order);
            await producer.ProduceAsync(_config["Kafka:COrder"], new Message<Null, string> { Value = ticketOrder });

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order == null) return NotFound();
            return Ok(order);
        }
    }
}
