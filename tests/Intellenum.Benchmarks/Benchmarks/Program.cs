// See https://aka.ms/new-console-template for more information

using Ardalis.SmartEnum;
using BenchmarkDotNet.Running;
using Intellenum;
using NetEscapades.EnumGenerators;

Console.WriteLine(ECustomerType.Standard < ECustomerType.Gold); 
Console.WriteLine(ECustomerType.Gold < ECustomerType.Standard); 

Console.WriteLine(EGCustomerType.Standard < EGCustomerType.Gold); 

Console.WriteLine(SECustomerType.Standard < SECustomerType.Gold); 

Console.WriteLine(SmartStringString.Standard < SmartStringString.Gold); 

Console.WriteLine(IECustomerType.Standard < IECustomerType.Gold);


EGCustomerTypeExtensions.IsDefined((EGCustomerType) 666);

BenchmarkRunner.Run(new[]
{
    typeof(FromValueBenchmarks)
});
// BenchmarkRunner.Run(new[]
// {
//     typeof(ToStringBenchmarks),
//     typeof(TryFromNameValueBenchmarks),
//     typeof(ContainsValueBenchmarks),
//     typeof(FromValueBenchmarks)
// });


public enum EDecimal : decimal
{
    Standard,
    Gold,
    Diamond,
    Platinum
}

public enum ECustomerType
{
    Standard,
    Gold,
    Diamond,
    Platinum
}

[EnumExtensions]
public enum EGCustomerType
{
    Standard,
    Gold,
    Diamond,
    Platinum
}

[Intellenum]
[Instance("Standard", 0)]
[Instance("Gold", 1)]
[Instance("Diamond",2)]
[Instance("Platinum", 3)]
public partial class IECustomerType
{
}

public class SmartStringString : SmartEnum<SmartStringString, string>
{
    public static readonly SmartStringString Standard = new(nameof(Standard), "Standard");
    public static readonly SmartStringString Gold = new(nameof(Gold), "Gold");

    public SmartStringString(string name, string value) : base(name, value)
    {
    }
}

public class SECustomerType : SmartEnum<SECustomerType>
{
    public static readonly SECustomerType Standard = new(nameof(Standard), 0);
    public static readonly SECustomerType Gold = new(nameof(Gold), 1);
    public static readonly SECustomerType Diamond = new(nameof(Diamond), 2);
    public static readonly SECustomerType Platinum = new(nameof(Platinum), 3);
        
    public SECustomerType(string name, int value) : base(name, value)
    {
    }
}