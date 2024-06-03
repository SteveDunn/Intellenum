namespace Intellenum.Tests.Types;

[Intellenum<int>]
[Members("Member1,Member2,      Member3")]
public partial class UsingMembersAttribute;


[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class Anything
{
}