using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.FromNameAndValue;

[Intellenum]
public partial class VendorType
{
    static VendorType() => Members("Standard, Preferred, Blocked");
}

[UsedImplicitly]
internal class FromNameAndValue : IScenario
{
    public Task Run()
    {
        RunFromName();
        RunTryFromName();
        RunFromValue();
        RunTryFromValue();

        return Task.CompletedTask;
    }

    private static void RunFromName()
    {
        Console.WriteLine($$"""VendorType.FromName("Standard") = {{VendorType.FromName("Standard")}}""");

        Run("Wibble");
        Run("Standard");

        return;

        static void Run(string name)
        {
            try
            {
                VendorType.FromName(name);
            }
            catch (IntellenumMatchFailedException e)
            {
                Console.WriteLine("FromName failed -" + e);
            }
        }
    }

    private static void RunFromValue()
    {
        Console.WriteLine($$"""VendorType.FromValue(1) = {{VendorType.FromValue(1)}}""");

        Run(1);
        Run(99);

        return;

        static void Run(int value)
        {
            try
            {
                VendorType.FromValue(value);
            }
            catch (IntellenumMatchFailedException e)
            {
                Console.WriteLine("FromValue failed -" + e);
            }
        }
    }

    private static void RunTryFromName()
    {
        Run("Wibble");
        Run("Standard");

        return;

        static void Run(string name)
        {
            bool found = VendorType.TryFromName(name, out _);
            if (!found)
            {
                Console.WriteLine($"TryFromName(\"{name}\") failed");
            }
            else
            {
                Console.WriteLine($"TryFromName(\"{name}\") succeeded");
            }
        }
    }

    private static void RunTryFromValue()
    {
        Run(1);
        Run(99);

        return;

        static void Run(int value)
        {
            bool found = VendorType.TryFromValue(value, out _);
            if (!found)
            {
                Console.WriteLine($"TryFromValue({value}) failed");
            }
            else
            {
                Console.WriteLine($"TryFromValue({value}) succeeded");
            }
        }
    }
}