using System;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.Lists;

internal class ListsScenario : IScenario
{
    public Task Run()
    {
        var l = Result.List();
        
        Console.WriteLine("List has the following members:");
        foreach (var item in l)
        {
            Console.WriteLine(item);
        }
        
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
