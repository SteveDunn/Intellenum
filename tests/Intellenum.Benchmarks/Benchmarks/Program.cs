// See https://aka.ms/new-console-template for more information

using Ardalis.SmartEnum;
using BenchmarkDotNet.Running;
using Intellenum;
using NetEscapades.EnumGenerators;

BenchmarkRunner.Run<TryFromNameValueBenchmarks>();


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