namespace Intellenum.Tests.Types;

[Intellenum(typeof(int))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class MyInt
{
}

#if NET7_0_OR_GREATER
[Intellenum<int>]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class MyIntGeneric
{
}
#endif