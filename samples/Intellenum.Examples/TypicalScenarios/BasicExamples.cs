using System;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.Basic
{
    internal class BasicExamples : IScenario
    {
        public Task Run()
        {
            // below are some example declarations of Intellenums

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

    // can be internal class
    [Intellenum]
    [Member("Accepted", 0)]
    [Member("Shipped", 1)]
    internal partial class OrderStatus
    {
    }

    // can be internal sealed
    [Intellenum]
    [Member("Warm", 1)]
    [Member("Bright", 5)]
    internal sealed partial class LumenType
    {
    }

    [Intellenum]
    [Member("Standard", 0)]
    [Member("Gold", 1)]
    
    public partial class CustomerType
    {
    }

    [Intellenum]
    [Member("Standard", 0)]
    [Member("Preferred", 1)]

    public partial class SupplierType
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
}