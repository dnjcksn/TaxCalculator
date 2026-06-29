namespace TaxCalculator.Application;

using System.Threading.Tasks;
using TaxCalculator.Domain;

public class TaxCalculatorService : ITaxCalculatorService
{

    private const int MonthsPerYear = 12;
    private const int CurrencyDecimalPlaces = 2;

    private readonly ITaxBandRepository _taxBandRepository;

    public TaxCalculatorService(ITaxBandRepository taxBandRepository)
    {
        _taxBandRepository = taxBandRepository;
    }

    public async Task<TaxSummary> Calculate(decimal grossAnnualSalary)
    {
        var taxBands = await _taxBandRepository.GetAllAsync();
        var annualTax = CalculateAnnualTax(grossAnnualSalary, taxBands);
        var netAnnualSalary = grossAnnualSalary - annualTax;
        decimal grossMonthlySalary = Math.Round(grossAnnualSalary / MonthsPerYear, CurrencyDecimalPlaces);

        decimal netMonthlySalary = Math.Round(netAnnualSalary / MonthsPerYear, CurrencyDecimalPlaces);
        decimal monthlyTax = Math.Round(annualTax / MonthsPerYear, CurrencyDecimalPlaces);

        return new TaxSummary
        {
            GrossAnnualSalary = grossAnnualSalary,
            GrossMonthlySalary = grossMonthlySalary,
            NetAnnualSalary = netAnnualSalary,
            NetMonthlySalary = netMonthlySalary,
            AnnualTax = annualTax,
            MonthlyTax = monthlyTax,
        };
    }

    private decimal CalculateAnnualTax(decimal grossAnnualSalary, List<TaxBand> taxBands)
    {
        return 0;
    }
}