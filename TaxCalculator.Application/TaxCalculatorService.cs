namespace TaxCalculator.Application;

using System.Threading.Tasks;
using TaxCalculator.Domain;

public class TaxCalculatorService : ITaxCalculatorService
{
    private readonly ITaxBandRepository _taxBandRepository;

    public TaxCalculatorService(ITaxBandRepository taxBandRepository)
    {
        _taxBandRepository = taxBandRepository;
    }

    public Task<TaxSummary> Calculate(decimal grossAnnualSalary)
    {
        throw new NotImplementedException();
    }
}