namespace ConsumerTests.DeserializationTests;

[Intellenum(typeof(int), Conversions.TypeConverter)]
[Instance("Item1", 1)]
[Instance("Item2", 2)]
public partial class Vo_AllowValidAndKnownInstances
{
}
