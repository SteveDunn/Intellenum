using System;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.ExplicitCasting
{
    internal class ExplicitCastingScenario : IScenario
    {
        public Task Run()
        {
            // We can create an instance with an explicit cast. If there is validation, it is still run.
            Result score1 = (Result)2;
            Result score2 = Result.FromValue(2);
            
            Console.WriteLine(score1 == score2); // true
            Console.WriteLine(score1 == Result.Won); // true
            
            // We can cast an instance to the underlying type too
            int score3 = (int) score2;
            Console.WriteLine(score3 == score2); // true

            return Task.CompletedTask;
        }
    }

    // defaults to int
    [Intellenum]
    [Instance("Won", 2)]
    [Instance("Drawn", 1)]
    [Instance("Lost", 0)]
    internal partial class Result
    {
    }
}