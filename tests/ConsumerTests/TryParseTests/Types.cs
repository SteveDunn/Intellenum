namespace ConsumerTests.TryParseTests;

[Intellenum(typeof(int))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class IntVoNoValidation
{
}

[Intellenum(typeof(int))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class IntEnum
{
}

[Intellenum(typeof(byte))]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class ByteVo { }

[Intellenum(typeof(char))]
[Instance("Item1", 'a')]
[Instance("Item2", 2)]
public partial class CharVo { }

[Intellenum(typeof(decimal))]
public partial class DecimalEnum
{
    static DecimalEnum()
    {
        Instance("Item1", 1.23m);
        Instance("Item2", 3.21m);
    }
}

[Intellenum(typeof(double))]
[Instance("Item1", 1.23d)]
[Instance("Item2", 3.21d)]
public partial class DoubleEnum { }