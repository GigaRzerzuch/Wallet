using Microsoft.EntityFrameworkCore;
using Wallet.Server.Models;

namespace Wallet.Server.Data;

public partial class ApplicationDbContext : DbContext
{
    private readonly string _connectionString;

    public ApplicationDbContext()
    {
        _connectionString = GetConnectionString();
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        _connectionString = GetConnectionString();
    }

    public virtual DbSet<Trade> Trades { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_connectionString);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Trade>(entity =>
        {
            entity.Property(e => e.TradeDate).HasDefaultValueSql("(getdate())");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    private string GetConnectionString()
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var config = builder.Build();
        return config.GetConnectionString("DefaultConnection");
    }
}
