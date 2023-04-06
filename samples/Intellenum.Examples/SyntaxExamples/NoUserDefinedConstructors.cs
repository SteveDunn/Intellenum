namespace Intellenum.Examples.SyntaxExamples.NoUserDefinedConstructors
{
    /*
        You shouldn't be allowed to use a default constructor as it could bypass
        any validation you might have added.
    */

    [Intellenum]
    [Instance("Standard", 1)]
    [Instance("Gold", 2)]
    public partial class CustomerType
    {
        // uncomment - error VOG008: Cannot have user defined constructors, please use the From method for creation.
        // public CustomerType() { }
        // public CustomerType(int value) { }
        // public CustomerType(int v1, int v2) : this(v1) { }
    }

    [Intellenum]
    [Instance("Standard", 1)]
    [Instance("Preferred", 2)]
    public partial class VendorType
    {
        // uncomment - error VOG008: Cannot have user defined constructors, please use the From method for creation.
        // public VendorType() { }
        // public VendorType(int value) { }
        // public VendorType(int v1, int v2) : this(v1) { }
        // public VendorType(int v1, int v2, int v3) : this(v1) { }
        // public VendorType(int v1, int v2, int v3, int v4) : this(v1) { }
    }
}