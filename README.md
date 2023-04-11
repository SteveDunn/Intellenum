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
It generates backing code for extremely fast, and allocation-free, lookups, with the `FromName` and `FromValue` methods (and the equivalent `Try...` methods).

The benefits of using Intellenum over standard enums is speed for the times when you need to see if an enum has a member of a particular name or value.
Benchmarks are provided below, but here is a snippet showing the performance gains for using `IsDefined`:

| Method          | Mean          | Error       | StdDev      | Median       | Gen0   | Allocated |
|-----------------|---------------|-------------|-------------|--------------|--------|-----------|
| StandardEnums   | 107.4646 ns   | 1.1617 ns   | 1.0867 ns   | 107.3232 ns  | 0.0057 | 96 B      |
| **Intellenums** | **0.0022 ns** | **0.0031 ns**   | **0.0027 ns**   | **0.0010 ns**    | **-**      | **-**         |


## Usage

Using Intellenum is as easy as importing the namespace and using the provided API:

```csharp
[Intellenum]
public partial class CustomerType
{
    public static readonly CustomerType Standard = new(1);
    public static readonly CustomerType Gold = new(2);
}
```

Note that you **don't need to repeat the member name** as it is inferred from the field name at compile time. But if you 
wanted a different name, you can specify it with
```csharp
public static readonly CustomerType Standard = new("STD", 1);
```
By default, the underlying type is `int`, but you can specify a different type, e.g. `[Intellenum(typeof(short))]` or the generic version `[Intellenum<short>]`.

As well as explicitly declaring members like above, there are a couple of other ways - here is an example:

```csharp
[Intellenum]
public partial class CustomerType
{
    static CustomerType()
    {
        Member("Standard", 1);
        Member("Gold", 2);
    }
}
```

Another way is via attributes:

```csharp
[Intellenum]
[Member("Standard", 1)]
[Member("Gold", 2)]
public partial class CustomerType { }
```

... or mix them up!

```csharp
[Intellenum]
[Member("Standard", 1)]
public partial class CustomerType 
{
    public static readonly CustomerType Gold = new CustomerType(2);
    public static readonly CustomerType Diamond = new CustomerType(3);

    static CustomerType()
    {
        Member("Platinum", 4);
    }
 }
```

... you can then treat the type just like an enum:

```csharp
if(type == CustomerType.Standard) Reject();
if(type == CustomerType.Gold) Accept();
```
        
## A look at the generated code
For constant values, a switch expression is generated for `IsDefined`:

```csharp
public static bool IsDefined(System.Int32 value)
{
    return value switch { 
      1 => true,
      2 => true,
      3 => true,
      4 => true,
      _ => false
    };
}
```
For `FromValue` (and `TryFromValue`), a switch statement is used:
```csharp
public static bool TryFromValue(System.Int32 value, out CustomerType member)
{
  switch (value) 
  {
      case 1:
          member = CustomerType.Unspecified; 
          return true;
      case 2:
          member = CustomerType.Normal; 
          return true;
      case 3:
          member = CustomerType.Gold; 
          return true;
      case 4:
          member = CustomerType.Diamond; 
          return true;
      default:
          member = default;
          return false;
  }
}
```

As an aside, we experimented with using switch expressions for this too, but they turned out to be slower (~ 2x) than normal
switch statements due to the need for having a tuple in the expression. 

The generated code look like this:
```csharp
public static bool TryFromValue2(int value, out CustomerType member)
{
    Func<(CustomerType, bool)> f = value switch
    {
        1 => () => (CustomerType.Unspecified, true), 
        2 => () => (CustomerType.Normal, true), 
        3 => () => (CustomerType.Gold, true), 
        4 => () => (CustomerType.Diamond, true), 
        _ => () => (default, false)
    };
    
    var r = f();
    member = r.Item1;
    return r.Item2;
}
```

_If you can think of a way of making this faster, please let us know!_



## Features

* `FromName()` and `FromValue()` (and `TryFrom...`)
* Can declare members in various ways:
  * attributes `[Member("Foo", 1)]`
  * explicitly with `new MyEnum("Foo", 1)`
  * Member method in a static constructor: `Member("Foo", 1);`
* Ability to quickly convert enums to and from strings
* Ability to quickly deconstruct enums to name and value, e.g. Deconstruct (`var (name, value) = CustomerType.Gold`)
* List items (`CustomerTypes.List()`)

## Installation

Intellenum can be installed using NuGet:

```
Install-Package Intellenum
```

# Comparison with other libraries
The bulk of Intellenum is based on the work done for Vogen which is a source generator for value objects. One of the features of Vogen
is the ability to specify 'instances'. These instances are very similar to the members of an enum, but they are not enums.
I had a few requests to use the same source generation and analyzers used for Vogen but to generate enums instead. This is what Intellenum is.

There are a few other libraries for dealing with enums. Some, for example, SmartEnum, declare a base class containing functionality.
Others, e.g. EnumGenerators, use attributes on standard enums to generate source code. 

Intellenum is a mixture of both. It uses an attribute to specify an 'enum' and then source-generates the functionality.




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

| Method         | Mean        | Error      | StdDev     | Allocated | Example |
|----------------|------------|------------|------------|-----------|------|
| StandardEnums  | 123.937 ns | 0.5615 ns  | 0.4977 ns  | -         |                   `Enum.TryParse<CustomerType>("Standard", out _)` |
| EnumGenerators | 9.067 ns   | 0.0523 ns  | 0.0489 ns  | -         |                   `CustomerTypeExtensions.TryParse("Standard", out _)` |
| SmartEnums     | 30.719 ns  | 0.4043 ns  | 0.3782 ns  | -         |                   `CustomerType.TryFromName( "Gold", out _)` |
| **Intellenums**    | **11.460 ns** | **0.2545 ns** | **0.2380 ns** | **-**         |   **`CustomerType.TryFromName("Standard", out _)`** |

### `Value`
_note that EnumGenerators isn't here as we use the standard C# enum to get its value_

| Method         | Mean       | Error      | StdDev     | Allocated |
|----------------|-----------|------------|------------|-----------|
| StandardEnums  | 0.0092 ns | 0.0082 ns  | 0.0077 ns  | -         |
| SmartEnums     | 0.3246 ns | 0.0082 ns  | 0.0069 ns  | -         |
| **Intellenums**   | **0.3198 ns** | **0.0103 ns** | **0.0096 ns** | **-**         |

### What does `ToString` return?
It returns the **name** of the member.
There is also a TypeConverter; when this is asked to convert a member to a `string',
it returns the **value** of the member as a string.

### What can the `TypeConverters` convert to and from?
They can convert an underlying type back to a matching enum.

### Can it serialize/deserialize?
Yes, it can. There's various ways to do this, including:
* System.Text.Json
* Newtonsoft.Json
* Dapper
* Entity Framework Core
* Linq2Db
* TypeConverters

Right now, Intellenum serializes using the `Value` property just like native enums.



> NOTE: Intellenum is in beta at the moment; I've tested it out and I think it works. The main functionality is present and the API probably 
> won't change significantly from now on. Although it's a fairly new library, it borrows a lot of code and features from [Vogen](https://github.com/SteveDunn/Vogen) which 
> has been in use for a while now by many projects and has lots of downloads, which should provide some confidence.
> Please feel free to try it and provide feedback.
