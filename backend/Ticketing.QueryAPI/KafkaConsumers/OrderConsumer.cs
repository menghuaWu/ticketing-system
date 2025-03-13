
using AutoMapper;
using Confluent.Kafka;
using Newtonsoft.Json;
using Ticketing.QueryAPI.Models;
using Ticketing.QueryAPI.Services;

namespace Ticketing.QueryAPI.KafkaConsumers
{
    public class OrderConsumer : BackgroundService
    {
        private readonly OrderService _orderService;
        private readonly IConfiguration _config;
        private readonly IConsumer<Null, string> _consumer;
        private readonly ILogger<OrderConsumer> _logger;

        public OrderConsumer(ILogger<OrderConsumer> logger, OrderService orderService, IConfiguration config, IMapper mapper)
        {
            _logger = logger;
            _orderService = orderService;
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
                    _consumer.Subscribe(_config["Kafka:COrder"]);
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
            //_mapper = mapper;

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
                        var orderDTO = JsonConvert.DeserializeObject<OrderDTO>(ticketJson);
                        _logger.LogInformation($"Received message: {orderDTO}");
                        if (orderDTO != null)
                        {
                            //DTO資料映射至mongoDB
                           var order = new Order
                           {
                               MssqlId = orderDTO.Id, // 儲存MSSQL產出的ID
                               TicketId = orderDTO.TicketId,
                               UserId = orderDTO.UserId,
                               Status = orderDTO.Status,
                               CreatedAt = orderDTO.CreatedAt
                           };
                            //var ticket = _mapper.Map<Ticket>(ticketDTO);
                            await _orderService.CreateAsync(order);
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
    }
}
