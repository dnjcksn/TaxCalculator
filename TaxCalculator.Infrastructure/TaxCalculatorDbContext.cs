namespace TaxCalculator.Infrastructure;

using Microsoft.EntityFrameworkCore;
using TaxCalculator.Domain;

public class TaxCalculatorDbContext : DbContext
{
    public TaxCalculatorDbContext(DbContextOptions<TaxCalculatorDbContext> options) : base(options)
    {

    }

    public DbSet<TaxBand> TaxBands { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(TaxCalculatorDbContext).Assembly
        );
    }
}