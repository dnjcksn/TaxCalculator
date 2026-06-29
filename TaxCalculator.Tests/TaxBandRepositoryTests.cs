using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using TaxCalculator.Infrastructure;

public class TaxBandRepositoryTests
{
    private IDbContextFactory<TaxCalculatorDbContext> CreateInMemoryFactory()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<TaxCalculatorDbContext>()
            .UseSqlite(connection)
            .Options;

        var context = new TaxCalculatorDbContext(options);
        context.Database.EnsureCreated();

        return new TestDbContextFactory(options);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsCorrectBands()
    {
        var factory = CreateInMemoryFactory();
        var repository = new TaxBandRepository(factory);

        var taxBands = await repository.GetAllAsync();

        taxBands.Should().Contain(b => b.Name == "Band A" && b.LowerLimit == 0 && b.TaxRate == 0);
        taxBands.Should().Contain(b => b.Name == "Band B" && b.LowerLimit == 5000 && b.TaxRate == 20);
        taxBands.Should().Contain(b => b.Name == "Band C" && b.LowerLimit == 20000 && b.TaxRate == 40);
    }

}