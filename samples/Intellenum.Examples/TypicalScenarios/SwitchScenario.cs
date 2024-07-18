using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.Switch;

[UsedImplicitly]
internal class SwitchScenario : IScenario
{
    public Task Run()
    {
        foreach (var vendorType in VendorType.List())
        {
            string shortCode = vendorType.Value switch
            {
                VendorType.StandardValue => "STD",
                VendorType.PreferredValue => "PRFRD",
                VendorType.BlockedValue => "BLCKED",
                _ => throw new InvalidOperationException("Unknown vendor type")
            };

            Console.WriteLine($"The short name for the vendor {vendorType} is {shortCode}");
        }

        return Task.CompletedTask;
    }
}

[Intellenum]
public partial class VendorType
{
    static VendorType() => Members("Standard, Preferred, Blocked");
}