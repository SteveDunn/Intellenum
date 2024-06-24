namespace ConsumerTests.SerializationAndConversionTests.Types;

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(bool))]
[Member("No", false)]
[Member("Yes", true)]
public partial class SsdtBoolVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(byte))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class SsdtByteVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(char))]
[Member("A", 'a')]
[Member("B", 'b')]
public partial class SsdtCharVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(DateOnly))]
public partial class SsdtDateOnlyVo
{
    static SsdtDateOnlyVo()
    {
        Member("JanFirst", new DateOnly(2021, 1, 1));
        Member("JanSecond", new DateOnly(2021, 1, 2));
    }
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(DateTimeOffset))]
public partial class SsdtDateTimeOffsetVo
{
    static SsdtDateTimeOffsetVo()
    {
        Member("JanFirst", new DateTimeOffset(2019, 1, 1, 14, 15, 16, TimeSpan.Zero));
        Member("JanSecond", new DateTimeOffset(2019, 1, 2, 14, 15, 16, TimeSpan.Zero));
    }
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(float))]
[Member("Item1", 1.1f)]
[Member("Item2", 2.2f)]
public partial class SsdtFloatVo { }


[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(DateTime))]
public partial class SsdtDateTimeVo
{
    static SsdtDateTimeVo()
    {
        Member("Item1", new DateTime(2019, 12, 13, 14, 15, 16));
        Member("Item2", new DateTime(2020, 12, 13, 14, 15, 16));
    }
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(decimal))]
public partial class SsdtDecimalVo
{
    static SsdtDecimalVo()
    {
        Member("Item1", 1.1m);
        Member("Item2", 2.2m);
    }
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(double))]
[Member("Item1", 1.1d)]
[Member("Item2", 2.2d)]
public partial class SsdtDoubleVo
{
}

public record struct Bar(int Age, string Name) : IComparable<Bar>
{
    public int CompareTo(Bar other) => Age.CompareTo(other.Age);
}


// [Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(Bar))]
// public partial class SsdtFooVo
// {
//     static SsdtFooVo()
//     {
//         Member("Fred", new Bar(42, "Fred"));
//         Member("Wilma", new Bar(40, "Wilma"));
//     }
//
//     public static SsdtFooVo Parse(string s) => throw new Exception("todo!");
// }

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(Guid))]
public partial class SsdtGuidVo
{
    static SsdtGuidVo()
    {
        Member("Item1", new Guid("00000000-0000-0000-0000-000000000001"));
        Member("Item2", new Guid("00000000-0000-0000-0000-000000000002"));
    }
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(int))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class SsdtIntVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(long))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class SsdtLongVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(short))]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class SsdtShortVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(string))]
[Member("Item1", "1")]
[Member("Item2", "2")]
public partial class SsdtStringVo
{
}

[Intellenum(conversions: Conversions.ServiceStackDotText, underlyingType: typeof(TimeOnly))]
public partial class SsdtTimeOnlyVo
{
    static SsdtTimeOnlyVo()
    {
        Member("Item1", new TimeOnly(1, 2, 3, 04));
        Member("Item2", new TimeOnly(5, 6, 7, 08));
    }
}