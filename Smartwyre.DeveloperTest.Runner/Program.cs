using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using System;

namespace Smartwyre.DeveloperTest.Runner;

class Program
{
    static void Main(string[] args)
    {
        Console.Write("Rebate calculation");

        Console.Write("Enter RebateIdentifier: ");
        var rebateIdentifier = Console.ReadLine();

        Console.Write("Enter ProductIdentifier: ");
        var productIdentifier = Console.ReadLine();

        Console.Write("Enter Volume: ");
        var volume = Console.ReadLine();
        if (!decimal.TryParse(volume, out var vol)) throw new ArgumentException(nameof(volume));

        // some Dependency Injection could be used here
        var rebateDataStore = new RebateDataStore();
        var productDataStore = new ProductDataStore();
        var service = new RebateService(rebateDataStore, productDataStore);

        var result = service.Calculate(new() { RebateIdentifier = rebateIdentifier, ProductIdentifier = productIdentifier, Volume = vol });
        if (result.Success) 
        {
            Console.Write($"Rebate calculation was successful. Amount = {result.Result.Amount}");
        } 
        else 
        {
            Console.Write("Rebate calculation was unsuccessful");
        }
    }
}
