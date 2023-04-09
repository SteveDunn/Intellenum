namespace ConsumerTests.ToStringTests;

public class Derivation
{
    [Fact]
    public void ToString_uses_users_method() => 
        TheEnum.Item1.ToString().Should().Be("ToString_Vo!");

    [Fact]
    public void ToString_uses_derived_method() => 
        Vod1.Item3.ToString().Should().Be("derived1!");

    [Fact]
    public void ToString_uses_least_derived_method() => 
        Vod2.Item5.ToString().Should().Be("derived2!");
}

public class D1
{
    public override string ToString() => "derived1!";
}

public class D2 : D1
{
    public new virtual string ToString() => "derived2!";
}

public class D3 : D2
{
}

[Intellenum]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class TheEnum
{
    public override string ToString() => "ToString_Vo!";
}

[Intellenum]
[Member("Item3", 3)]
[Member("Item4", 4)]
public partial class Vod1 : D1
{
}

[Intellenum]
[Member("Item5", 5)]
[Member("Item6", 6)]
public partial class Vod2 : D3
{
}

public record R1
{
    public override string ToString() => "R1!";
}
