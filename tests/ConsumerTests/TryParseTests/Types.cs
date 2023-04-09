namespace ConsumerTests.TryParseTests;

[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class IntVoNoValidation
{
}

[Intellenum(typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class IntEnum
{
}

[Intellenum(typeof(byte))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class ByteVo { }

[Intellenum(typeof(char))]
[Member("Item1", 'a')]
[Member("Item2", 2)]
public partial class CharVo { }

[Intellenum(typeof(decimal))]
public partial class DecimalEnum
{
    static DecimalEnum()
    {
        Member("Item1", 1.23m);
        Member("Item2", 3.21m);
    }
}

[Intellenum(typeof(double))]
[Member("Item1", 1.23d)]
[Member("Item2", 3.21d)]
public partial class DoubleEnum { }