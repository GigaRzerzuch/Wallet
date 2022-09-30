using Microsoft.EntityFrameworkCore;
using Wallet.Shared.Models;

namespace Wallet.Server.Data
{
    public class DataContext : DbContext
    {

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Trade> Trades{ get; set; }
        public DbSet<User> Users{ get; set; }
    }
}
