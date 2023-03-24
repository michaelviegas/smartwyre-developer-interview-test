using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;
public class CalculateRebateResultTests
{
    [Fact]
    public void ShouldSuccessBeFalse_WhenNoConstructorParameters()
    {
        // Arrange
        var result = new CalculateRebateResult();

        // Act & Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void ShouldSuccessBeFalse_WhenRebateCalculationIsNull()
    {
        // Arrange
        var result = new CalculateRebateResult(null);

        // Act & Assert
        Assert.False(result.Success);
    }

    [Fact]
    public void SuccessShouldBeTrue_WhenRebateCalculationIsProvided()
    {
        // Arrange
        var rebateCalculation = new RebateCalculation(new(), new(), 0);
        var result = new CalculateRebateResult(rebateCalculation);

        // Act & Assert
        Assert.True(result.Success);
        Assert.Equal(rebateCalculation, result.Result);
    }
}
