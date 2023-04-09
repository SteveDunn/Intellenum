namespace Intellenum.Tests.Types;

[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class MyInt
{
}

#if NET7_0_OR_GREATER
[Intellenum<int>]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class MyIntGeneric
{
}
#endif