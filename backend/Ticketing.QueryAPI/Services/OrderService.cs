using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ticketing.QueryAPI.Data;
using Ticketing.QueryAPI.Models;

namespace Ticketing.QueryAPI.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order> _orderCollection;

        public OrderService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _orderCollection = mongoDatabase.GetCollection<Order>(mongoDbSettings.Value.OrderCollection);
        }

        public async Task<List<Order>> GetAllAsync() =>
            await _orderCollection.Find(_ => true).ToListAsync();

        public async Task<Order?> GetByIdAsync(string id) =>
            await _orderCollection.Find(t => t.Id.ToString() == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Order ticket) =>
            await _orderCollection.InsertOneAsync(ticket);
    }
}
