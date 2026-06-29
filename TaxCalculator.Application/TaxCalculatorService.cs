namespace TaxCalculator.Application;

using Microsoft.Extensions.Logging;
using TaxCalculator.Domain;

public class TaxCalculatorService : ITaxCalculatorService
{

    private const int MonthsPerYear = 12;
    private const int CurrencyDecimalPlaces = 2;

    private readonly ITaxBandRepository _taxBandRepository;
    private readonly ILogger<TaxCalculatorService> _logger;

    public TaxCalculatorService(ITaxBandRepository taxBandRepository, ILogger<TaxCalculatorService> logger)
    {
        _taxBandRepository = taxBandRepository;
        _logger = logger;
    }

    public async Task<TaxSummary> Calculate(decimal grossAnnualSalary)
    {
        _logger.LogInformation("Calculating tax summary for gross annual salary {GrossAnnualSalary}", grossAnnualSalary);

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
        var annualTax = 0m;
        var rangedTaxBands = ToRangedTaxBands(taxBands);

        foreach (RangedTaxBand rangedTaxBand in rangedTaxBands)
        {
            if (grossAnnualSalary > rangedTaxBand.TaxBand.LowerLimit)
            {
                var upperLimit = GetUpperLimit(rangedTaxBand, grossAnnualSalary);
                var taxBandAmount = upperLimit - rangedTaxBand.TaxBand.LowerLimit;
                var taxBandPercentage = rangedTaxBand.TaxBand.TaxRate / 100m;
                var tax = taxBandAmount * taxBandPercentage;
                _logger.LogDebug(
                    "Calculated tax for {Band}: {LowerLimit}-{UpperLimit}, {Rate}% of {Amount} = {Tax}",
                    rangedTaxBand.TaxBand.Name,
                    rangedTaxBand.TaxBand.LowerLimit,
                    upperLimit,
                    rangedTaxBand.TaxBand.TaxRate,
                    taxBandAmount,
                    tax
                );
                annualTax += tax;
            }
        }

        _logger.LogInformation("Annual tax calculated as {AnnualTax}", annualTax);

        return annualTax;
    }

    private IEnumerable<RangedTaxBand> ToRangedTaxBands(List<TaxBand> taxBands)
    {
        var rangedTaxBands = new List<RangedTaxBand>();

        var orderedTaxBands = OrderTaxBands(taxBands);
        var noOfTaxBands = orderedTaxBands.Count;

        for (int taxBandIndex = 0; taxBandIndex < noOfTaxBands; taxBandIndex++)
        {
            var taxBand = orderedTaxBands[taxBandIndex];
            var nextTaxBandIndex = taxBandIndex + 1;

            var upperLimit = nextTaxBandIndex < noOfTaxBands
                ? orderedTaxBands[nextTaxBandIndex].LowerLimit
                : (int?)null;

            rangedTaxBands.Add(new RangedTaxBand
            {
                TaxBand = taxBand,
                UpperLimit = upperLimit,
            }
            );
        }

        return rangedTaxBands;
    }

    private List<TaxBand> OrderTaxBands(List<TaxBand> taxBands)
    {
        return taxBands.OrderBy(b => b.LowerLimit).ToList();
    }

    private decimal GetUpperLimit(RangedTaxBand rangedTaxBand, decimal grossAnnualSalary)
    {
        if (rangedTaxBand.UpperLimit is null)
        {
            return grossAnnualSalary;
        }
        if (rangedTaxBand.UpperLimit > grossAnnualSalary)
        {
            return grossAnnualSalary;
        }
        return rangedTaxBand.UpperLimit.Value;
    }

    /// <summary>
    /// A tax band with an upper limit.
    /// Used to temporarily store the upper limit which aids in tax calculations.
    /// </summary>
    private class RangedTaxBand
    {
        public required TaxBand TaxBand { get; set; }
        public int? UpperLimit { get; set; }
    }
}