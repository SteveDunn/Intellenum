namespace ConsumerTests.DeserializationTests;

[Intellenum(typeof(int), Conversions.TypeConverter)]
[Member("Item1", 1)]
[Member("Item2", 2)]
public partial class Enum_AllowValidAndKnownInstances
{
}
