using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Ticketing.QueryAPI.Data;
using Ticketing.QueryAPI.Models;

namespace Ticketing.QueryAPI.Services
{
    public class TicketService
    {
        private readonly IMongoCollection<Ticket> _ticketCollection;

        public TicketService(IOptions<MongoDbSettings> mongoDbSettings)
        {
            var mongoClient = new MongoClient(mongoDbSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbSettings.Value.DatabaseName);
            _ticketCollection = mongoDatabase.GetCollection<Ticket>(mongoDbSettings.Value.CollectionName);
        }

        public async Task<List<Ticket>> GetAllAsync() =>
            await _ticketCollection.Find(_ => true).ToListAsync();

        public async Task<Ticket?> GetByIdAsync(string id) =>
            await _ticketCollection.Find(t => t.Id.ToString() == id).FirstOrDefaultAsync();

        public async Task CreateAsync(Ticket ticket) =>
            await _ticketCollection.InsertOneAsync(ticket);
    }
}
