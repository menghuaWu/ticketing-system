using Confluent.Kafka;
using System.Text.Json;
using Ticketing.QueryAPI.Models;
using Ticketing.QueryAPI.Services;

namespace Ticketing.QueryAPI.KafkaConsumers
{
    public class TicketConsumer : BackgroundService
    {
        private readonly TicketService _ticketService;
        private readonly IConfiguration _config;
        private readonly IConsumer<Null, string> _consumer;

        public TicketConsumer(TicketService ticketService, IConfiguration config)
        {
            _ticketService = ticketService;
            _config = config;

            var consumerConfig = new ConsumerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"],
                GroupId = _config["Kafka:GroupId"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
            _consumer.Subscribe(_config["Kafka:Topic"]);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var consumeResult = _consumer.Consume(stoppingToken);
                var ticket = JsonSerializer.Deserialize<Ticket>(consumeResult.Message.Value);
                if (ticket != null)
                {
                    await _ticketService.CreateAsync(ticket);
                }
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
