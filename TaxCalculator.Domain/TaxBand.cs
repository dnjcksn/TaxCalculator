namespace TaxCalculator.Domain;

public class TaxBand
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int LowerLimit { get; set; }
    public int TaxRate { get; set; }
}