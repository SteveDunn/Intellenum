using System;
using System.Threading.Tasks;

// ReSharper disable RedundantCast

namespace Intellenum.Examples.TypicalScenarios.Equality
{
    internal class EqualityExamples : IScenario
    {
        public Task Run()
        {

            // error CS0019: Operator '==' cannot be applied to operands of type 'Age' and 'Centigrade'
            // Console.WriteLine(Age.From(1) == Centigrade.From(1)); // true

            return Task.CompletedTask;
        }
    }

    [Intellenum<decimal>]
    public partial class Centigrade
    {
        static Centigrade()
        {
            Instance("AbsoluteZero", -273.15m);
            Instance("FreezingPointOfWater", 0m);
            Instance("BoilingPointOfWater", 100m);
        }
    }
}