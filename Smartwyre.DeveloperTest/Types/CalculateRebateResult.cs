namespace Smartwyre.DeveloperTest.Types;

public class CalculateRebateResult
{
    public CalculateRebateResult() : this(default) { }
    public CalculateRebateResult(RebateCalculation result) 
    {
        Result = result;
    }

    public bool Success => Result != default;
    public RebateCalculation Result { get; }
}
