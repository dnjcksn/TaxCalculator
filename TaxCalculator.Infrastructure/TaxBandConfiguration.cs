namespace TaxCalculator.Infrastructure;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TaxCalculator.Domain;

public class TaxBandConfiguration : IEntityTypeConfiguration<TaxBand>
{
    public void Configure(EntityTypeBuilder<TaxBand> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.LowerLimit)
            .IsRequired();

        builder.Property(t => t.TaxRate)
            .IsRequired();

        builder.HasData(
            new TaxBand
            {
                Id = 1,
                Name = "Band A",
                LowerLimit = 0,
                TaxRate = 0
            },
            new TaxBand
            {
                Id = 2,
                Name = "Band B",
                LowerLimit = 5000,
                TaxRate = 20
            },
            new TaxBand
            {
                Id = 3,
                Name = "Band C",
                LowerLimit = 20000,
                TaxRate = 40
            }
        );
    }
}