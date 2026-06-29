using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using TaxCalculator.Domain;
using TaxCalculator.Application;

public class TaxCalculatorServiceTests
{
    private readonly List<TaxBand> _taxBands = new()
    {
        new TaxBand { Name = "Band A", LowerLimit = 0,     TaxRate = 0  },
        new TaxBand { Name = "Band B", LowerLimit = 5000,  TaxRate = 20 },
        new TaxBand { Name = "Band C", LowerLimit = 20000, TaxRate = 40 },
    };

    private readonly TaxCalculatorService _service;

    public TaxCalculatorServiceTests()
    {
        var mockTaxBandRepository = new Mock<ITaxBandRepository>();
        mockTaxBandRepository
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(_taxBands);

        _service = new TaxCalculatorService(mockTaxBandRepository.Object, new NullLogger<TaxCalculatorService>());
    }

    [Fact]
    public async Task Calculate_Salary10000_Returns1000()
    {
        var result = await _service.Calculate(10000);
        result.GrossAnnualSalary.Should().Be(10000);
        result.GrossMonthlySalary.Should().Be(833.33m);
        result.NetAnnualSalary.Should().Be(9000);
        result.NetMonthlySalary.Should().Be(750.00m);
        result.AnnualTax.Should().Be(1000);
        result.MonthlyTax.Should().Be(83.33m);
    }

    [Fact]
    public async Task Calculate_Salary40000_Returns11000()
    {
        var result = await _service.Calculate(40000);
        result.GrossAnnualSalary.Should().Be(40000);
        result.GrossMonthlySalary.Should().Be(3333.33m);
        result.NetAnnualSalary.Should().Be(29000);
        result.NetMonthlySalary.Should().Be(2416.67m);
        result.AnnualTax.Should().Be(11000);
        result.MonthlyTax.Should().Be(916.67m);
    }

    [Fact]
    public async Task Calculate_Salary1000_Returns0()
    {
        var result = await _service.Calculate(1000);
        result.GrossAnnualSalary.Should().Be(1000);
        result.GrossMonthlySalary.Should().Be(83.33m);
        result.NetAnnualSalary.Should().Be(1000);
        result.NetMonthlySalary.Should().Be(83.33m);
        result.AnnualTax.Should().Be(0);
        result.MonthlyTax.Should().Be(0);
    }
}