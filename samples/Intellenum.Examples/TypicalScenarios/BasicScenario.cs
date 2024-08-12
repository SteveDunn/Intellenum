using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Intellenum.Examples.TypicalScenarios.Basic;

[UsedImplicitly]
internal class BasicScenario : IScenario
{
    public Task Run()
    {
        // below are some example declarations of Intellenums

        return Task.CompletedTask;
    }
}

// defaults to int
[Intellenum]
internal partial class Result
{
    public static readonly Result Won, Drawn, Lost;
}

// can be internal class
[Intellenum]
internal partial class OrderStatus
{
    public static readonly OrderStatus Accepted, Shipped;
}

// can be internal sealed
[Intellenum]
[Member("Warm", 1)]
[Member("Bright", 5)]
internal sealed partial class LumenTypeWithAttributes;

[Intellenum]
internal sealed partial class LumenTypeWithFields
{
    public static readonly LumenTypeWithFields 
        Warm = new(1), 
        Bright = new(5);
}

[Intellenum]
[Member("Standard", 0)]
[Member("Gold", 1)]
    
public partial class CustomerTypeWithAttributes
{
}

[Intellenum]
public partial class CustomerTypeWithFields
{
    public static readonly CustomerTypeWithFields Standard, Gold;
}

[Intellenum]
public partial class SupplierType
{
    public static readonly SupplierType Standard, Preferred;
}
    
[Intellenum<Planet>]
public partial class PlanetEnum
{
    public static readonly PlanetEnum 
        Jupiter = new(new Planet("Brown", 273_400)),
        Mars = new(new Planet("Red", 13_240)),
        Venus = new(new Planet("White", 23_622));
}

public record Planet(string Colour, int CircumferenceInMiles) : IComparable<Planet>
{
    public int CompareTo(Planet other) => CircumferenceInMiles.CompareTo(other.CircumferenceInMiles);
}

