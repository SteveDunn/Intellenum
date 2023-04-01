| Name           | Operating System      | Status                                                                              | History                                                                                                                                                                            |
|:---------------|:----------------------|:------------------------------------------------------------------------------------|:-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| GitHub Actions | Ubuntu, Mac & Windows | ![Build](https://github.com/stevedunn/intellenum/actions/workflows/build.yaml/badge.svg) | [![GitHub Actions Build History](https://buildstats.info/github/chart/SteveDunn/intellenum?branch=main&includeBuildsFromPullRequest=false)](https://github.com/SteveDunn/intellenum/actions) |

 [![GitHub release](https://img.shields.io/github/release/stevedunn/intellenum.svg)](https://GitHub.com/stevedunn/intellenum/releases/) [![GitHub license](https://img.shields.io/github/license/stevedunn/intellenum.svg)](https://github.com/SteveDunn/intellenum/blob/main/LICENSE) 
[![GitHub issues](https://img.shields.io/github/issues/Naereen/StrapDown.js.svg)](https://GitHub.com/stevedunn/intellenum/issues/) [![GitHub issues-closed](https://img.shields.io/github/issues-closed/Naereen/StrapDown.js.svg)](https://GitHub.com/stevedunn/intellenum/issues?q=is%3Aissue+is%3Aclosed) 
[![NuGet Badge](https://buildstats.info/nuget/intellenum)](https://www.nuget.org/packages/Intellenum/)

<p align="center">
  <img src="./assets/intellenum.png" alt="The Intellenum logo">
</p>

[![Sparkline](https://stars.medv.io/stevedunn/intellenum.svg)](https://stars.medv.io/stevedunn/intellenum)
## Give a Star! :star:
If you like or are using this project please give it a star. Thanks!

# Intellenum: intelligence, for your enums!

Intellenum is an open source project written in C# that provides a fast and efficient way to deal with enums. 
It uses source generation to make dealing with enums faster and easier than ever.

Given the following code:

```csharp
[Intellenum]
public partial class CustomerType
{
    static CustomerType()
    {
        Instance("Standard", 1);
        Instance("Gold", 2);
    }
}
```
... you can then treat the type just like an enum:

```csharp
if(type = CustomerType.Standard) Reject();
if(type = CustomerType.Gold) Accept();
```

Intellenum generates backing code for lightning fast and allocation-free lookups, e.g. `FromName`, `FromValue` (and their equivalent `Try...` methods):

There are other ways to declare instances of the enum. You can declare them directly:
```csharp
[Intellenum]
public partial class CustomerType
{
    public static readonly CustomerType Standard = new CustomerType("Standard", 1);
    public static readonly CustomerType Gold = new CustomerType("Gold", 2);
}
```

... or you can use attributes:

```csharp
[Intellenum]
[Instance("Standard", 1)]
[Instance("Gold", 2)]
public partial class CustomerType { }
```

... or mix them up!

```csharp
[Intellenum]
[Instance("Standard", 1)]
public partial class CustomerType 
{
    public static readonly CustomerType Standard = new CustomerType("Gold", 2);

    static CustomerType()
    {
        Instance("Diamond", 3);
    }
 }
```

## Features

* `FromName()` and `FromValue()` (and `TryFrom...`)
* Generates enum definitions in various ways:
  * attributes `[Instance("Foo", 1)]`
  * explicitly with `new MyEnum("Foo", 1)`
  * instance method in a static constructor: `Instance("Foo", 1);`
* Ability to quickly convert enums to and from strings
* Ability to quickly deconstruct enums to name and value
* Deconstruct (`(name, value) = CustomerTypes.Gold`)
* Get items (`CustomerTypes.List()`)

## Installation

Intellenum can be installed using NuGet:

```
Install-Package Intellenum
```

## Usage

Using Intellenum is as easy as importing the namespace and using the provided API:

```csharp
using Intellenum;

[Intellenum]
public partial class CustomerType
{
    static CustomerType() 
    {
        Instance("Standard", 1);
        Instance("Gold", 2);
    }
}

// some tests

        CustomerType t1 = CustomerType.Standard;
        CustomerType t2 = CustomerType.Gold;

        CustomerType x = CustomerType.FromValue(1);

        (t1 == t2).Should().BeFalse();

        (t1 == x).Should().BeTrue();

        CustomerType.FromValue(1).Should().Be(CustomerType.Standard);
        CustomerType.FromValue(2).Should().Be(CustomerType.Gold);

        CustomerType.FromName("Standard").Should().Be(CustomerType.Standard);
        CustomerType.FromName("Gold").Should().Be(CustomerType.Gold);

        CustomerType.ContainsValue(1).Should().BeTrue();
        CustomerType.ContainsValue(2).Should().BeTrue();
        CustomerType.ContainsValue(3).Should().BeFalse();

        CustomerType ct1;
        CustomerType.TryFromName("Standard", out ct1).Should().BeTrue();

        CustomerType ct2;
        CustomerType.TryFromName("Gold", out ct2).Should().BeTrue();

        CustomerType ct3;
        CustomerType.TryFromName("FOO", out ct3).Should().BeFalse();

        CustomerType ctv1;
        CustomerType.TryFromValue(1, out ctv1).Should().BeTrue();
        ctv1.Should().Be(CustomerType.Standard);

        CustomerType ctv2;
        CustomerType.TryFromValue(2, out ctv2).Should().BeTrue();
        ctv2.Should().Be(CustomerType.Gold);

        CustomerType.TryFromValue(666, out _).Should().BeFalse();

```

# FAQ

## How fast is it?

Very fast! Here's some comparisons of various libraries (and the default `enum` in C#) 

### `IsDefined`
... or `TryFromValue`

| Method          | Mean          | Error       | StdDev      | Median       | Gen0   | Allocated |
|-----------------|---------------|-------------|-------------|--------------|--------|-----------|
| StandardEnums   | 107.4646 ns   | 1.1617 ns   | 1.0867 ns   | 107.3232 ns  | 0.0057 | 96 B      |
| EnumGenerators  | 0.0113 ns     | 0.0108 ns   | 0.0101 ns   | 0.0095 ns    | -      | -         |
| SmartEnums      | 13.1542 ns    | 0.0863 ns   | 0.0720 ns   | 13.1441 ns   | -      | -         |
| **Intellenums** | **0.0022 ns** | **0.0031 ns**   | **0.0027 ns**   | **0.0010 ns**    | **-**      | **-**         |

### `ToString()`

| Method          | Mean          | Error       | StdDev      | Gen0   | Allocated  |
|-----------------|---------------|-------------|-------------|--------|------------|
| StandardEnums   | 11.9803 ns    | 0.0961 ns   | 0.0852 ns   | 0.0014 | 24 B       |
| EnumGenerators  | 1.5292 ns     | 0.0230 ns   | 0.0215 ns   | -      | -          |
| SmartEnums      | 0.8921 ns     | 0.0109 ns   | 0.0096 ns   | -      | -          |
| **Intellenums** | **0.8934 ns** | **0.0193 ns**   | **0.0180 ns**   | **-**      | **-**          |

### `FromName()`

| Method         | Mean        | Error      | StdDev     | Allocated |
|----------------|------------|------------|------------|-----------|
| StandardEnums  | 123.937 ns | 0.5615 ns  | 0.4977 ns  | -         |
| EnumGenerators | 9.067 ns   | 0.0523 ns  | 0.0489 ns  | -         |
| SmartEnums     | 30.719 ns  | 0.4043 ns  | 0.3782 ns  | -         |
| **Intellenums**    | **11.460 ns** | **0.2545 ns** | **0.2380 ns** | **-**         |

### `Value`
_note that EnumGenerators isn't here as we use the standard C# enum to get its value_
| Method         | Mean       | Error      | StdDev     | Allocated |
|----------------|-----------|------------|------------|-----------|
| StandardEnums  | 0.0092 ns | 0.0082 ns  | 0.0077 ns  | -         |
| SmartEnums     | 0.3246 ns | 0.0082 ns  | 0.0069 ns  | -         |
| **Intellenums**   | **0.3198 ns** | **0.0103 ns** | **0.0096 ns** | **-**         |

### What does `ToString` return?
It returns the **name** of the instance.
There is also a TypeConverter; when this is asked to convert an instance to a `string',
it returns the **value** of the instance as a string.

### What can the `TypeConverters` convert to and from?
They can convert an underlying type back to a matching enum. 


> NOTE: Intellenum is in pre-release at the moment, so probably isn't production ready and the API might (and probably will) change.
> But feel free to kick the tyres and provide feedback. It's not far off being complete as it borrows a lot of code and features from [Vogen](https://github.com/SteveDunn/Vogen)
