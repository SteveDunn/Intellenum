namespace Intellenum.Tests.Types;

[Intellenum<int>]
[Members("Member1,Member2,      Member3")]
public partial class UsingMembersAttribute;

[Intellenum]
[Members("Standard, Gold")]
[Member("Diamond", 2)]
public partial class CustomerType2 
{
    static CustomerType2()
    {
        Member("Platinum", 3);
    }
    public static readonly CustomerType Royalty = new(4);
}


[Intellenum<int>]
[Members("Member1,Member2,      Member3")]
[Member("Member4", 3)]
public partial class UsingAMixtureOfEverything
{
    static UsingAMixtureOfEverything()
    {
        Member("Member5", 4);
        Member("Member6", 5);
    }

    public static readonly UsingAMixtureOfEverything Member7 = new("Member7", 6);
}


[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class Anything
{
}