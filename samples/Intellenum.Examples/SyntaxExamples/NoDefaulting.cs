// ReSharper disable UnusedVariable
#pragma warning disable CS0219

namespace Intellenum.Examples.SyntaxExamples.NoDefaulting
{
    /*
        You shouldn't be allowed to `default` a Intellenums as you should always
        chose a valid instance.
    */
    
    public class Naughty
    {
        public Naughty()
        {
            // uncomment for - error VOG009: Type 'CustomerType' cannot be constructed with default as it is prohibited.
            // CustomerType c = default; 
            // var c2 = default(CustomerType);

            // VendorId v = default;
            // var v2 = default(VendorId);

            // uncomment for - error VOG010: Type 'VendorId' cannot be constructed with 'new' as it is prohibited.
            // var v3 = new VendorId();

            // uncomment for - error VOG010: Type 'CustomerType' cannot be constructed with 'new' as it is prohibited.
            //var v4 = new CustomerType();
            // CustomerType v5 = new();
            // var _ = new CustomerType();
            // new CustomerType();
        }

        // a method can't accept a Intellenum and default it
        // error VOG009: Type 'CustomerType' cannot be constructed with default as it is prohibited
        // public void CallMe(CustomerType customerId = default)
        // {
        //     int _ = customerId.Value;
        // }

        // error VOG009: Type 'CustomerType' cannot be constructed with default as it is prohibited.
        // public CustomerType GetCustomerType() => default;

        //  error VOG010: Type 'CustomerType' cannot be constructed with 'new' as it is prohibited.
        // public CustomerType GetCustomerType() => new();
        // public CustomerType GetCustomerType() => new CustomerType();
    }

    [Intellenum]
    [Instance("Standard", 1)]
    [Instance("Gold", 2)]
    public partial class CustomerType { }

    [Intellenum<string>]
    [Instance("Good", "Good")]
    [Instance("Average", "Average")]
    [Instance("Bad", "Bad")]
    public partial class VendorRating { }
}