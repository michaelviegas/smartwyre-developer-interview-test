using Xunit;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateCalculationTests
{
    [Fact]
    public void RebateCalculation_Should_Return_Correct_Values()
    {
        // Arrange
        var rebate = new Rebate
        {
            Identifier = "R001",
            Incentive = IncentiveType.AmountPerUom            
        };
        var product = new Product
        { 
            Id = 1,
            Identifier = "P001"
        };
        var amount = 10.0m;

        // Act
        var rebateCalculation = new RebateCalculation(rebate, product, amount);

        // Assert
        Assert.Equal(1, rebateCalculation.Id);
        Assert.Equal("P001", rebateCalculation.Identifier);
        Assert.Equal("R001", rebateCalculation.RebateIdentifier);
        Assert.Equal(IncentiveType.AmountPerUom, rebateCalculation.IncentiveType);
        Assert.Equal(10.0m, rebateCalculation.Amount);
    }
}