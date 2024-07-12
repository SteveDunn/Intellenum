using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Intellenum.Examples.TypicalScenarios.ExplicitCasting;

[UsedImplicitly]
internal class ExplicitCastingScenario : IScenario
{
    public Task Run()
    {
            // We can create an instance with an explicit cast. If there is validation, it is still run.
            Result result1 = (Result)2;
            Result result2 = Result.FromValue(2);
            
            Console.WriteLine(result1 == result2); // true
            Console.WriteLine(result1 == Result.Won); // true
            
            // We can cast an instance to the underlying type too
            int score3 = (int) result2;
            Console.WriteLine(score3 == result2); // true

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