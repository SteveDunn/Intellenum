using System;
using System.Threading.Tasks;

// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.Equality
{
    internal class EqualityExamples : IScenario
    {
        public Task Run()
        {
            Console.WriteLine(Centigrade.AbsoluteZero == Centigrade.FromName("AbsoluteZero")); // true
            Console.WriteLine(Centigrade.AbsoluteZero == Centigrade.FromValue(-273.15m)); // true
            Console.WriteLine(Centigrade.AbsoluteZero == -273.15m); // true

            return Task.CompletedTask;
        }
    }

    [Intellenum<decimal>]
    public partial class Centigrade
    {
        static Centigrade()
        {
            Member("AbsoluteZero", -273.15m);
            Member("FreezingPointOfWater", 0m);
            Member("BoilingPointOfWater", 100m);
        }
    }
}