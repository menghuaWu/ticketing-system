using AutoMapper;
using Confluent.Kafka;
using MongoDB.Bson;
using Newtonsoft.Json;
using Ticketing.QueryAPI.Models;
using Ticketing.QueryAPI.Services;

namespace Ticketing.QueryAPI.KafkaConsumers
{
    public class TicketConsumer : BackgroundService
    {
        private readonly TicketService _ticketService;
        private readonly IConfiguration _config;
        private readonly IConsumer<Null, string> _consumer;
        private readonly ILogger<TicketConsumer> _logger;
        private readonly IMapper _mapper;

        public TicketConsumer(ILogger<TicketConsumer> logger, TicketService ticketService, IConfiguration config, IMapper mapper)
        {
            _logger = logger;
            _ticketService = ticketService;
            _config = config;
            try
            {
                var consumerConfig = new ConsumerConfig
                {
                    BootstrapServers = _config["Kafka:BootstrapServers"],
                    GroupId = _config["Kafka:GroupId"],
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };
                Console.WriteLine($"Kafka設定: {JsonConvert.SerializeObject(consumerConfig)}");
                _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
                try
                {
                    _consumer.Subscribe(_config["Kafka:Topic"]);
                    Console.WriteLine($"Kafka初始化");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"等待 Kafka 準備好，錯誤: {e.Message}");
                    Thread.Sleep(5000);  // 等待 5 秒再重試
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"kafka初始化錯誤: {ex.Message}");
                _logger.LogError(ex, "Failed to connect to Kafka.");
            }
            _mapper = mapper;
            
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                try
                {
                    Console.WriteLine($"執行ExecuteAsync");
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        Console.WriteLine($"開始消費...");
                        var consumeResult = _consumer.Consume(stoppingToken);
                        var ticketJson = consumeResult.Message.Value;
                        Console.WriteLine($"接收到消息: {ticketJson}");
                        // kafka資料取出轉為DTO
                        var ticketDTO = JsonConvert.DeserializeObject<TicketDTO>(ticketJson);
                        _logger.LogInformation($"Received message: {ticketDTO}");
                        if (ticketDTO != null)
                        {
                            // DTO資料映射至mongoDB
                            //var ticket = new Ticket
                            //{
                            //    MssqlId = ticketDTO.Id, // 儲存MSSQL產出的ID
                            //    Title = ticketDTO.Title,
                            //    Price = ticketDTO.Price
                            //};
                            var ticket = _mapper.Map<Ticket>(ticketDTO);
                            await _ticketService.CreateAsync(ticket);
                        }
                    }
                    Console.WriteLine($"結束ExecuteAsync");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"接收到消息有錯誤: {ex.Message}");
                    _logger.LogError(ex, "Error occurred while consuming messages.");
                    //throw;
                }
            });
            
            
        }

        public override void Dispose()
        {
            Console.WriteLine($"Dispose...");
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
