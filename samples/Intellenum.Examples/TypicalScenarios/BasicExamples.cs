using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Intellenum.Examples.TypicalScenarios.Basic;

[UsedImplicitly]
internal class BasicExamples : IScenario
{
    public Task Run()
    {
        // below are some example declarations of Intellenums

        return Task.CompletedTask;
    }
}

// defaults to int
[Intellenum]
[Member("Won", 2)]
[Member("Drawn", 1)]
[Member("Lost", 0)]
internal partial class Result
{
}

// can be internal class
[Intellenum]
[Member("Accepted", 0)]
[Member("Shipped", 1)]
internal partial class OrderStatus
{
}

// can be internal sealed
[Intellenum]
[Member("Warm", 1)]
[Member("Bright", 5)]
internal sealed partial class LumenType
{
}

[Intellenum]
[Member("Standard", 0)]
[Member("Gold", 1)]
    
public partial class CustomerType
{
}

[Intellenum]
[Member("Standard", 0)]
[Member("Preferred", 1)]

public partial class SupplierType
{
}
    
[Intellenum<Planet>]
public partial class PlanetEnum
{
    public static readonly PlanetEnum Jupiter = new(new Planet("Brown", 273_400));
    public static readonly PlanetEnum Mars=  new(new Planet("Red", 13_240));
    public static readonly PlanetEnum Venus=  new(new Planet("White", 23_622));
}

public record class Planet(string Colour, int CircumferenceInMiles) : IComparable<Planet>
{
    public int CompareTo(Planet other) => CircumferenceInMiles.CompareTo(other.CircumferenceInMiles);
}