#nullable disable

using System.Text.RegularExpressions;

namespace ConsumerTests.TryParseTests;

[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class IntVoNoValidation
{
}

[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class IntEnum
{
}

[Intellenum(typeof(byte))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class ByteVo { }

[Intellenum(typeof(char))]
[Member("Item1", 'a')]
[Member("Item2", 2)]
public partial class CharVo { }

[Intellenum(typeof(decimal))]
public partial class DecimalEnum
{
    static DecimalEnum()
    {
        Member("Item1", 1.23m);
        Member("Item2", 3.21m);
    }
}

[Intellenum(typeof(double))]
[Member("Item1", 1.23d)]
[Member("Item2", 3.21d)]
public partial class DoubleEnum { }

[Intellenum<Planet>]
public partial class PlanetEnum
{
    public static readonly PlanetEnum
        Jupiter = new(new Planet("Brown", 273_400)),
        Mars = new(new Planet("Red", 13_240)),
        Venus = new(new Planet("White", 23_622));
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
            result = default;
            return false;
        }

        string colour = match.Groups["colour"].Value;
        string circumference = match.Groups["circumference"].Value;

        result = new Planet(colour, Convert.ToInt32(circumference));

        return true;
    }
}