using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Types;
using System;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;

    public RebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);

        if (rebate == null || product == null) return new();

        decimal rebateAmount;
        switch (rebate.Incentive)
        {
            case IncentiveType.FixedCashAmount:
                if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedCashAmount)
                    || rebate.Amount == 0) return new();

                rebateAmount = rebate.Amount;
                break;

            case IncentiveType.FixedRateRebate:
                if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.FixedRateRebate) ||
                    rebate.Percentage == 0 ||
                    product.Price == 0 ||
                    request.Volume == 0) return new();

                rebateAmount = product.Price * rebate.Percentage * request.Volume;
                break;

            case IncentiveType.AmountPerUom:
                if (!product.SupportedIncentives.HasFlag(SupportedIncentiveType.AmountPerUom) ||
                    rebate.Amount == 0 ||
                    request.Volume == 0) return new();

                rebateAmount = rebate.Amount * request.Volume;
                break;

            default: return new();
        }

        _rebateDataStore.StoreCalculationResult(rebate, rebateAmount);
        return new(new RebateCalculation(rebate, product, rebateAmount));
    }
}
