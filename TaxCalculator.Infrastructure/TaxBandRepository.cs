namespace TaxCalculator.Infrastructure;

using Microsoft.EntityFrameworkCore;
using TaxCalculator.Application;
using TaxCalculator.Domain;

public class TaxBandRepository : ITaxBandRepository
{
    private readonly IDbContextFactory<TaxCalculatorDbContext> _contextFactory;

    public TaxBandRepository(IDbContextFactory<TaxCalculatorDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<TaxBand>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.TaxBands.ToListAsync();
    }
}