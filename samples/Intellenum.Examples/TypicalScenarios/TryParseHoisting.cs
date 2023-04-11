using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.TryParseHoisting
{
    internal class TryParseHoistingExample : IScenario
    {
        public Task Run()
        {
            // The underlying type of PlanetEnum is Planet, which has a static TryParse method

            {
                bool r = PlanetEnum.TryParse("Brown-273400", out var p);
                Console.WriteLine(r); // true
                Console.WriteLine(p.ToString()); // Jupiter
            }

            {
                bool r = PlanetEnum.TryParse("Blue-24901", out _);
                Console.WriteLine(r); // false
            }


            Console.WriteLine(PlanetEnum.Mars < PlanetEnum.Jupiter); // true
            
            Console.WriteLine(string.Join(", ", PlanetEnum.List().OrderDescending())); // Jupiter, Venus, Mars

            return Task.CompletedTask;
        }
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
    }}