namespace TaxCalculator.Application;

using TaxCalculator.Domain;

public interface ITaxCalculatorService
{
    /// <summary>
    /// Calculates the tax summary for the given gross annual salary.
    /// </summary>
    /// <param name="grossAnnualSalary">The gross annual salary.</param>
    /// <returns>The tax summary.</returns>
    Task<TaxSummary> Calculate(decimal grossAnnualSalary);
}