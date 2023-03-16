using Intellenum;

namespace Scratch;

[Intellenum]
[Instance("Standard", 1)]
[Instance("Gold", 2)]
public partial class CustomerType
{
}

[Intellenum]
public partial class Condiment
{
    static Condiment()
    {
        Instance("Salt", 1);
        Instance("Pepper", 2);
    }
}

[Intellenum]
[Instance("Salt", 1)]
[Instance("Pepper", 2)]
public partial class CondimentMixedInstances
{
    public static readonly CondimentMixedInstances Mayo = new CondimentMixedInstances("Mayo", 5);
    public static readonly CondimentMixedInstances Ketchup = new CondimentMixedInstances("Ketchup", 6);

    static CondimentMixedInstances()
    {
        Instance("Vinegar", 3);
        Instance("Mustard", 4);
    }
}