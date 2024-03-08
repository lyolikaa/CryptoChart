using Microsoft.EntityFrameworkCore;

namespace DB;

public class CryptoContext : DbContext
{
    public CryptoContext(DbContextOptions<CryptoContext> options) : base(options)
    {
    }
    public DbSet<Snapshot> Snapshots { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=(local)\\SQLExpress;Initial Catalog=crypto;Persist Security Info=True;User ID=sa;Password=***;trusted_connection=true;encrypt=false");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Snapshot>().ToTable("Snapshots");
    }
}