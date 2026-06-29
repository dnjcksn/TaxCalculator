using Moq;
using FluentAssertions;
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

        _service = new TaxCalculatorService(mockTaxBandRepository.Object);
    }
}