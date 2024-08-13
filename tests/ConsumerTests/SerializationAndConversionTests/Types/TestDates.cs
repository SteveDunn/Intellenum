namespace ConsumerTests.TestEnums;

internal static class TestDates
{
    internal static readonly DateTime _date1 = new DateTime(1970, 6, 10, 14, 01, 02, DateTimeKind.Utc) + TimeSpan.FromTicks(12345678);
    internal static readonly DateTime _date2 = DateTime.Now.AddMinutes(42.69);
        
}