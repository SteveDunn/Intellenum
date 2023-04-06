using System;
using System.Threading.Tasks;

namespace Intellenum.Examples.TypicalScenarios.Basic
{
    internal class BasicExamples : IScenario
    {
        public Task Run()
        {
            // we can't mess up the order of parameters - doing the following results in:
            //      Argument 1: cannot convert from 'SupplierId' to 'CustomerId'
            // new CustomerProcessor().Process(SupplierId.From(123), SupplierId.From(321), Amount.From(123));

            new CustomerProcessor().Process(CustomerType.FromName("Standard"), SupplierType.FromValue(1));

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

    // can be internal class
    [Intellenum]
    [Instance("Accepted", 0)]
    [Instance("Shipped", 1)]
    internal partial class OrderStatus
    {
    }

    // can be internal sealed
    [Intellenum]
    [Instance("Warm", 1)]
    [Instance("Bright", 5)]
    internal sealed partial class LumenType
    {
    }

    [Intellenum]
    [Instance("Standard", 0)]
    [Instance("Gold", 1)]
    
    public partial class CustomerType
    {
    }

    [Intellenum]
    [Instance("Standard", 0)]
    [Instance("Preferred", 1)]

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


    internal class CustomerProcessor
    {
        internal void Process(CustomerType customerType, SupplierType supplierType) =>
            Console.WriteLine($"Processing customer {customerType}, supplier {supplierType}");
    }
}