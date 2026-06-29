namespace TaxCalculator.Application;

using TaxCalculator.Domain;

public interface ITaxBandRepository
{
    Task<List<TaxBand>> GetAllAsync();
}