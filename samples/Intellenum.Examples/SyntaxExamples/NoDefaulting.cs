// ReSharper disable UnusedVariable
#pragma warning disable CS0219

namespace Intellenum.Examples.SyntaxExamples.NoDefaulting
{
    /*
        You shouldn't be allowed to `default` a Intellenums as you should always
        chose a valid member.
    */
    
    public class Naughty
    {
        public Naughty()
        {
            // uncomment for - error INTELLENUM09: Type 'CustomerType' cannot be constructed with default as it is prohibited.
            // CustomerType c = default; 
            // var c2 = default(CustomerType);

            // uncomment for - error INTELLENUM010: Type 'VendorId' cannot be constructed with 'new' as it is prohibited.
            // var v3 = new CustomerType();
            // CustomerType v5 = new();
        }

        // a method can't accept a Intellenum and default it
        // error INTELLENUM09: Type 'CustomerType' cannot be constructed with default as it is prohibited
        // public static void CallMe(CustomerType customerId = default)
        // {
        //     int _ = customerId.Value;
        // }
    }

    [Intellenum]
    [Member("Standard", 1)]
    [Member("Gold", 2)]
    public partial class CustomerType { }

    [Intellenum<string>]
    [Member("Good", "Good")]
    [Member("Average", "Average")]
    [Member("Bad", "Bad")]
    public partial class VendorRating { }
}