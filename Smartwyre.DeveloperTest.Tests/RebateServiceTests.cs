using System;
using Xunit;
using Moq;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Tests;

public class RebateServiceTests
{
    private const string FixedCashAmountRebateId = "FixedCashAmountRebateId";
    private const string NoAmoutFixedCashAmountRebateId = "NoAmoutFixedCashAmountRebateId";
    private const string FixedRateRebateId = "FixedRateRebateId";
    private const string NoPercentageFixedRateRebateId = "NoPercentageFixedRateRebateId";
    private const string AmountPerUomRebateId = "AmountPerUomRebateId";
    private const string NoAmoutAmountPerUomRebateId = "NoAmoutAmountPerUomRebateId";

    private const string FixedCashAmountProductId = "FixedCashAmountProductId";
    private const string FixedRateRebateProductId = "FixedRateRebateProductId";
    private const string NoPriceFixedRateRebateProductId = "NoPriceFixedRateRebateProductId";
    private const string AmountPerUomProductId = "AmountPerUomProductId";

    private readonly RebateService _rebateService;

    public RebateServiceTests() 
    {
        var rebateDataStoreMock = new Mock<IRebateDataStore>();
        rebateDataStoreMock.Setup(x => x.GetRebate(It.Is<string>(i => i == FixedCashAmountRebateId))).Returns(new Rebate { Identifier = FixedCashAmountRebateId, Incentive = IncentiveType.FixedCashAmount, Amount = 10 });
        rebateDataStoreMock.Setup(x => x.GetRebate(It.Is<string>(i => i == NoAmoutFixedCashAmountRebateId))).Returns(new Rebate { Incentive = IncentiveType.FixedCashAmount, Amount = 0 });
        rebateDataStoreMock.Setup(x => x.GetRebate(It.Is<string>(i => i == FixedRateRebateId))).Returns(new Rebate { Identifier = FixedRateRebateId, Incentive = IncentiveType.FixedRateRebate, Percentage = 10 });
        rebateDataStoreMock.Setup(x => x.GetRebate(It.Is<string>(i => i == NoPercentageFixedRateRebateId))).Returns(new Rebate { Incentive = IncentiveType.FixedRateRebate, Percentage = 0 });
        rebateDataStoreMock.Setup(x => x.GetRebate(It.Is<string>(i => i == AmountPerUomRebateId))).Returns(new Rebate { Identifier = AmountPerUomRebateId, Incentive = IncentiveType.AmountPerUom, Amount = 10 });
        rebateDataStoreMock.Setup(x => x.GetRebate(It.Is<string>(i => i == NoAmoutAmountPerUomRebateId))).Returns(new Rebate { Incentive = IncentiveType.AmountPerUom, Amount = 0 });

        var productDataStoreMock = new Mock<IProductDataStore>();
        productDataStoreMock.Setup(x => x.GetProduct(It.Is<string>(i => i == FixedCashAmountProductId))).Returns(new Product { Identifier = FixedCashAmountProductId, SupportedIncentives = SupportedIncentiveType.FixedCashAmount });
        productDataStoreMock.Setup(x => x.GetProduct(It.Is<string>(i => i == FixedRateRebateProductId))).Returns(new Product { Identifier = FixedRateRebateProductId, SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 10 });
        productDataStoreMock.Setup(x => x.GetProduct(It.Is<string>(i => i == NoPriceFixedRateRebateProductId))).Returns(new Product { SupportedIncentives = SupportedIncentiveType.FixedRateRebate, Price = 0 });
        productDataStoreMock.Setup(x => x.GetProduct(It.Is<string>(i => i == AmountPerUomProductId))).Returns(new Product { Identifier = AmountPerUomProductId, SupportedIncentives = SupportedIncentiveType.AmountPerUom });

        _rebateService = new RebateService(rebateDataStoreMock.Object, productDataStoreMock.Object);
    }

    [Fact]
    public void Calculate_ShouldThrowArgumentNullException_WhenRequestIsNull() => Assert.Throws<ArgumentNullException>(() => _rebateService.Calculate(null));


    [Theory]
    [
        InlineData(FixedCashAmountRebateId, null), 
        InlineData(null, FixedCashAmountProductId),
        InlineData(FixedCashAmountRebateId, FixedRateRebateProductId),
        InlineData(FixedCashAmountRebateId, AmountPerUomProductId),
        InlineData(NoAmoutFixedCashAmountRebateId, FixedCashAmountProductId),
        InlineData(FixedRateRebateId, FixedCashAmountProductId, 10),
        InlineData(FixedRateRebateId, AmountPerUomProductId, 10),
        InlineData(NoPercentageFixedRateRebateId, FixedRateRebateProductId, 10),
        InlineData(FixedRateRebateId, NoPriceFixedRateRebateProductId, 10),
        InlineData(FixedRateRebateId, FixedRateRebateProductId),
        InlineData(AmountPerUomRebateId, FixedCashAmountProductId, 10),
        InlineData(AmountPerUomRebateId, FixedRateRebateProductId, 10),
        InlineData(NoAmoutAmountPerUomRebateId, AmountPerUomProductId, 10),
        InlineData(AmountPerUomRebateId, AmountPerUomProductId)
    ]
    public void Calculate_ShouldReturnUnsuccessfulCalculateRebateResult(string rebateId, string productId, decimal volume = 0)
    {
        // Act
        var result = _rebateService.Calculate(new() { RebateIdentifier = rebateId, ProductIdentifier = productId, Volume = volume });

        // Assert
        Assert.False(result.Success);
    }


    [Theory]
    [
        InlineData(FixedCashAmountRebateId, FixedCashAmountProductId, 0, 10),
        InlineData(FixedRateRebateId, FixedRateRebateProductId, 11, 1100),
        InlineData(AmountPerUomRebateId, AmountPerUomProductId, 22, 220)
    ]
    public void Calculate_ShouldReturnSuccessfulCalculateRebateResult(string rebateId, string productId, decimal volume, decimal expected)
    {
        // Act
        var result = _rebateService.Calculate(new() { RebateIdentifier = rebateId, ProductIdentifier = productId, Volume = volume });

        // Assert
        Assert.True(result.Success);
        Assert.Equal(rebateId, result.Result.RebateIdentifier);
        Assert.Equal(productId, result.Result.Identifier);
        Assert.Equal(expected, result.Result.Amount);
    }
}
