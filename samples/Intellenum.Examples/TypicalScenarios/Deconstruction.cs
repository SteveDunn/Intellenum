using System;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.Deconstruction;

internal class DeconstructionExamples : IScenario
{
    public Task Run()
    {
        var v = Result.Won;
            
        var (name, value) = v;
            
        Console.WriteLine($"Name is {name}, value is {value}");

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

[Intellenum<Foo>]
public partial class FooEnum
{
    public static readonly FooEnum Item1 = new("Item1", new Foo("a", 1));
    public static readonly FooEnum Item2=  new("Item2", new Foo("b", 2));
}

public record class Foo(string Name, int Age) : IComparable<Foo>
{
    public int CompareTo(Foo other) => Age.CompareTo(other.Age);
}


 