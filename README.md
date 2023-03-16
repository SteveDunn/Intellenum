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

> NOTE: Intellenum is in pre-release at the moment, so probably isn't production ready and the API might (and probably will change).
> But feel free to kick the tyres and provide feedback. It's not far off being complete as it borrows a lot of code and features from [Vogen](https://github.com/SteveDunn/Vogen)
