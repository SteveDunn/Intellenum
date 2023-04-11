using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.TryParseHoisting;


[UsesVerify]
public class Tests
{
    [Fact]
    public Task Test()
    {
        var source = $$"""
using System;
using Intellenum;
using System.Text.RegularExpressions;

namespace Whatever;

[Intellenum(typeof(Planet))]
public partial class PlanetEnum
{
    public static readonly PlanetEnum Jupiter = new(new Planet("Brown", 273_400));
    public static readonly PlanetEnum Mars=  new(new Planet("Red", 13_240));
    public static readonly PlanetEnum Venus=  new(new Planet("White", 23_622));
}

public record class Planet(string Colour, int CircumferenceInMiles) : IComparable<Planet>
{
    public int CompareTo(Planet other) => CircumferenceInMiles.CompareTo(other.CircumferenceInMiles);

    public static bool TryParse(string input, out Planet result)
    {
        string pattern = "^(?<colour>[a-zA-Z]+)-(?<circumference>\\d+)$";

        Match match = Regex.Match(input, pattern);

        if (!match.Success)
        {
            result = default!;
            return false;
        }

        string colour = match.Groups["colour"].Value;
        string circumference = match.Groups["circumference"].Value;

        result = new Planet(colour, Convert.ToInt32(circumference));

        return true;
    }
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOnAllFrameworks();
    }
}
