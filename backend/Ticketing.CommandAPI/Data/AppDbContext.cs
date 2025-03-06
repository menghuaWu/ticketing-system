using Microsoft.EntityFrameworkCore;
using System.Net.Sockets;
using Ticketing.CommandAPI.Models;

namespace Ticketing.CommandAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Ticket> Tickets => Set<Ticket>();
    }
}
