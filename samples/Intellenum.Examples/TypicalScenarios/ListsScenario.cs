using System;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.Lists;

[Intellenum]
internal partial class Result
{
    static Result() => Members("Lost, Drawn, Won");
}

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
