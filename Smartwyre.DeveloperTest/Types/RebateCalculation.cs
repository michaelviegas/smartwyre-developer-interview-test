namespace Smartwyre.DeveloperTest.Types;

public class RebateCalculation
{
    private readonly Rebate _rebate;
    private readonly Product _product;

    public RebateCalculation(Rebate rebate, Product product, decimal amount) 
    {
        _rebate = rebate;
        _product = product;
        Amount = amount;    
    }

    public int Id => _product.Id;
    public string Identifier => _product.Identifier;
    public string RebateIdentifier => _rebate.Identifier;
    public IncentiveType IncentiveType => _rebate.Incentive;
    public decimal Amount { get; }
}
