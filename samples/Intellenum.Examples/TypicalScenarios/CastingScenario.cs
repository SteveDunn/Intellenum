using System;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Intellenum.Examples.TypicalScenarios.Casting;

// defaults to int
[Intellenum]
internal partial class Result
{
    static Result() => Members("Lost, Drawn, Won");
}

[UsedImplicitly]
internal class CastingScenario : IScenario
{
    public Task Run()
    {
        ExplicitCasting();
        ImplicitCasting();

        return Task.CompletedTask;
    }

    private static void ExplicitCasting()
    {
        // We can create an instance with an explicit cast. If there is validation, it is still run.
        Result result1 = (Result)2;
        Result result2 = Result.FromValue(2);
            
        Console.WriteLine(result1 == result2); // true
        Console.WriteLine(result1 == Result.Won); // true
            
        // We can cast an instance to the underlying type too
        int score3 = (int) result2;
        Console.WriteLine(score3 == result2); // true
    }

    private static void ImplicitCasting()
    {
        // We can get the value with an implicit cast
        foreach (Result eachResult in Result.List())
        {
            int value = eachResult;
            Console.WriteLine($"{eachResult.Name} has a value of {value}");
        }
    }
}
