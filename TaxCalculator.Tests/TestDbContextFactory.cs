using Microsoft.EntityFrameworkCore;
using TaxCalculator.Infrastructure;

public class TestDbContextFactory : IDbContextFactory<TaxCalculatorDbContext>
{
    private readonly DbContextOptions<TaxCalculatorDbContext> _options;

    public TestDbContextFactory(DbContextOptions<TaxCalculatorDbContext> options)
    {
        _options = options;
    }

    public TaxCalculatorDbContext CreateDbContext()
    {
        return new TaxCalculatorDbContext(_options);
    }
}