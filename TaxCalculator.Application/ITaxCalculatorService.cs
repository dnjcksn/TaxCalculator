namespace TaxCalculator.Application;

using TaxCalculator.Domain;

public interface ITaxCalculatorService
{
    Task<TaxSummary> Calculate(decimal grossAnnualSalary);
}