using System;
using System.Linq;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.IComparable
{
    internal class IComparableExamples : IScenario
    {
        public Task Run()
        {
            // The underlying type of PlanetEnum is Planet, which implements IComparable<Planet> and
            // compares on circumference. This means that the Intellenum can be sorted.

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
    }
}