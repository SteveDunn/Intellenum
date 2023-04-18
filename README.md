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
# Give a Star!
:star: If you like or are using this project please give it a star. Thanks! :star:

# Intellenum: intelligence, for your enums!


<!-- TOC -->
  * [Give a Star!](#give-a-star)
* [Intellenum: intelligence, for your enums!](#intellenum-intelligence-for-your-enums)
  * [Overview](#overview)
  * [Installation](#installation)
  * [Usage](#usage)
    * [Configuration](#configuration)
    * [Underlying types](#underlying-types)
    * [Hoisting](#hoisting)
    * [FromName](#fromname)
    * [TryFromName](#tryfromname)
    * [FromValue](#fromvalue)
    * [TryFromValue](#tryfromvalue)
    * [List](#list)
    * [Deconstructing](#deconstructing)
    * [ToString](#tostring)
    * [Serialization](#serialization)
* [Comparison with other libraries](#comparison-with-other-libraries)
* [FAQ](#faq)
  * [How fast is it? ⚡](#how-fast-is-it-)
  * [What does `ToString` return?](#what-does-tostring-return)
  * [What can the `TypeConverters` convert to and from?](#what-can-the-typeconverters-convert-to-and-from)
  * [Can it serialize/deserialize?](#can-it-serializedeserialize)
  * [I use an Intellenum as a key in a Dictionary - can I serialize that dictionary?](#i-use-an-intellenum-as-a-key-in-a-dictionary---can-i-serialize-that-dictionary)
  * [A look at the generated code](#a-look-at-the-generated-code)
<!-- TOC -->


## Overview

Intellenum is an open source C# project that provides a fast and efficient way to deal with enums. 
It uses source generation that generates backing code for extremely fast, and allocation-free, lookups, 
with the `FromName` and `FromValue` methods (and the equivalent `Try...` methods).

Intellenum provides speed benefits over standard enums for when you need to see if an enum has a member 
of a particular name or value.
Benchmarks are provided below, but here is a snippet showing the performance gains for using `IsDefined`:

| Method          | Mean          | Error       | StdDev      | Median       | Gen0   | Allocated |
|-----------------|---------------|-------------|-------------|--------------|--------|-----------|
| StandardEnums   | 107.4646 ns   | 1.1617 ns   | 1.0867 ns   | 107.3232 ns  | 0.0057 | 96 B      |
| **Intellenums** | **0.0022 ns** | **0.0031 ns**   | **0.0027 ns**   | **0.0010 ns**    | **-**      | **-**         |


## Installation

Add the NuGet package to your project:

```
Install-Package Intellenum
```

## Usage

To get started, add a using for the `Intellenum` namespace and declare an enumeration like this:

```csharp
[Intellenum]
public partial class CustomerType
{
    public static readonly CustomerType Standard = new(1);
    public static readonly CustomerType Gold = new(2);
}
```

Note that you **don't need to repeat the member name** as it is inferred from the field name **at compile time**. 
You can also supply different name, e.g.:
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

### Configuration

Each Intellenum can have it's own *optional* configuration. Configuration includes:

* The underlying type
* Any 'conversions' (Dapper, System.Text.Json, Newtonsoft.Json, etc.) - see below for more information
* Any 'customization' (for instance, treating a number as string in JSON serialization)
* The type of the exception that is thrown when validation fails

If any of those above are not specified, then global configuration is inferred. It looks like this:

```csharp
[assembly: IntellenumDefaults(underlyingType: typeof(int), conversions: Conversions.Default)]
```

Those again are optional. If they're not specified, then they are defaulted to:

* Underlying type = `typeof(int)`
* Conversions = `Conversions.Default` (`TypeConverter` and `System.Text.Json`)
* Customizations = `Customizations.None`

### Underlying types
Supports underlying types such as `byte`, `sbyte`, `short`, `ushort`, `int`, `uint`, `long`, `ulong`, `char`, `string`, and `bool`. 

Also supports other types such as `Guid`, `DateTime`, `DateTimeOffset`, `TimeSpan`, `DateOnly`, `TimeOnly` etc.

You can also specify a custom type, e.g. `MyCustomType`. There are some restrictions for this custom type:
* It cannot be a collection
* It cannot be the same type as the enumeration itself

###  Hoisting
If the underlying type implements `IComparable`, then the generated enum code will also implement `IComparable`. 
The code that is generated will delegate to the underlying type's implementation. This means that you can use the `>` and `<` 
operators on the enum type. e.g.

```csharp
[Intellenum<Planet>]
public partial class PlanetEnum
{
    public static readonly PlanetEnum Jupiter = new(new Planet("Brown", 273_400));
    public static readonly PlanetEnum Mars=  new(new Planet("Red", 13_240));
    public static readonly PlanetEnum Venus=  new(new Planet("White", 23_622));
}

public record class Planet(string Colour, int CircumferenceInMiles) : IComparable<Planet>
{
    public int CompareTo(Planet other) => CircumferenceInMiles.CompareTo(other.CircumferenceInMiles);
}

Console.WriteLine(PlanetEnum.Mars < PlanetEnum.Jupiter); // true

Console.WriteLine(string.Join(", ", PlanetEnum.List().OrderDescending())); // Jupiter, Venus, Mars
```

Additionally, if the underlying type contains a static method named `TryParse`, then a `TryParse` method will be generated for the enum itself.
This `TryParse` method is useful if you want to find an enum by an alternative representation of its value.
The generated `TryParse` first calls the static `TryParse` on the underlying type, and then does a lookup with `TryFromValue`.
The code below demonstrates this. The enum has an underlying type of `Planet`, which has a `TryParse` method that parses a string in the format `'Colour-Circumference'.
Because the underlying type has a `TryParse` method, the generated enum also has a `TryParse` method which delegates to the underlying type's `TryParse` method:

```csharp
[Intellenum(typeof(Planet))]
public partial class PlanetEnum
{
    public static readonly PlanetEnum Jupiter = new(new Planet("Brown", 273_400));
    public static readonly PlanetEnum Mars=  new(new Planet("Red", 13_240));
    public static readonly PlanetEnum Venus=  new(new Planet("White", 23_622));
}

public record class Planet(string Colour, int CircumferenceInMiles)  : IComparable<Planet>
{
    public int CompareTo(Planet other) => CircumferenceInMiles.CompareTo(other.CircumferenceInMiles);

    public static bool TryParse(string input, out Planet result)
    {
        string pattern = "^(?<colour>[a-zA-Z]+)-(?<circumference>\\d+)$";

        Match match = Regex.Match(input, pattern);

        if (!match.Success)
        {
            result = default!;
            return false;
        }

        string colour = match.Groups["colour"].Value;
        string circumference = match.Groups["circumference"].Value;

        result = new Planet(colour, Convert.ToInt32(circumference));

        return true;
    }
 }

// the following tests pass    
{
    // this is defined
    bool r = PlanetEnum.TryParse("Brown-273400", out var p);
    r.Should().BeTrue();
    p.Should().Be(PlanetEnum.Jupiter);
}

{
    // this is not defined
    bool r = PlanetEnum.TryParse("Blue-24901", out _);
    r.Should().BeFalse();
}    
}
```
                  
### FromName
Gets the member of the enum with the specified name. If the name is not found, then a `IntellenumMatchFailedException` exception is thrown.

```csharp
var ct = CustomerType.FromName("Gold");
Console.WriteLine(ct.ToString()); // Gold
```

### TryFromName
Tries to get the instance of that name. Returns `true` if the name is found, otherwise `false`. Sets the output value if found.

```csharp
bool b = CustomerType.TryFromName("Gold", out var ct);
Console.WriteLine(b); // True
Console.WriteLine(ct.ToString()); // Gold
```

### FromValue
Tries to get the instance of that value. If not found, then a `IntellenumMatchFailedException` exception is thrown.

```csharp
var ct = CustomerType.FromValue(2);
Console.WriteLine(ct.ToString()); // Gold 
```

### TryFromValue
Tries to get the value. Returns true or false. Sets the output value if found.

```csharp
bool b = CustomerType.TryFromValue(2, out var ct);
Console.WriteLine(b); // True
Console.WriteLine(ct.ToString()); // Gold
```
### List
Returns an `IEnumerable<T>` of all the members of the enum. 

### Deconstructing

Deconstructs an enum into it's name and value. For example:

```csharp
var (name, value) = CustomerType.Gold;
Console.WriteLine(name); // Gold
Console.WriteLine(value); // 2
```
### ToString

The `ToString` method returns the name of the enum member. For example:

```csharp
Console.WriteLine(CustomerType.Gold); // Gold
```

### Serialization
Intellenum supports serialization to and from JSON using `System.Text.Json` and `Newtonsoft.Json`.
It also supports storing and retrieving from Dapper, EFCore and Linq2Db.


# Comparison with other libraries
The bulk of Intellenum is based on the work done for Vogen which is a source generator for value objects. One of the features of Vogen
is the ability to specify 'instances'. These instances are very similar to the members of an enum, but they are not enums.
There were a few requests to use the same source generation and analyzers used for Vogen but to generate enums instead. 
This is what Intellenum is.

There are a few other libraries for dealing with enums. Some, for example, SmartEnum, declare a base class containing functionality.
Others, e.g. EnumGenerators, use attributes on standard enums to generate source code. 

Intellenum is a mixture of both. It uses an attribute to specify an 'enum' and then source-generates the functionality.




# FAQ

## How fast is it? ⚡

Very fast! Here's some comparisons of various libraries (and the default `enum` in C#) 

* `IsDefined` ... or `TryFromValue`

| Method          | Mean          | Error       | StdDev      | Median       | Gen0   | Allocated |
|-----------------|---------------|-------------|-------------|--------------|--------|-----------|
| StandardEnums   | 107.4646 ns   | 1.1617 ns   | 1.0867 ns   | 107.3232 ns  | 0.0057 | 96 B      |
| EnumGenerators  | 0.0113 ns     | 0.0108 ns   | 0.0101 ns   | 0.0095 ns    | -      | -         |
| SmartEnums      | 13.1542 ns    | 0.0863 ns   | 0.0720 ns   | 13.1441 ns   | -      | -         |
| **Intellenums** | **0.0022 ns** | **0.0031 ns**   | **0.0027 ns**   | **0.0010 ns**    | **-**      | **-**         |

* `ToString()`

| Method          | Mean          | Error       | StdDev      | Gen0   | Allocated  |
|-----------------|---------------|-------------|-------------|--------|------------|
| StandardEnums   | 11.9803 ns    | 0.0961 ns   | 0.0852 ns   | 0.0014 | 24 B       |
| EnumGenerators  | 1.5292 ns     | 0.0230 ns   | 0.0215 ns   | -      | -          |
| SmartEnums      | 0.8921 ns     | 0.0109 ns   | 0.0096 ns   | -      | -          |
| **Intellenums** | **0.8934 ns** | **0.0193 ns**   | **0.0180 ns**   | **-**      | **-**          |

* `FromName()`

| Method         | Mean        | Error      | StdDev     | Allocated | Example |
|----------------|------------|------------|------------|-----------|------|
| StandardEnums  | 123.937 ns | 0.5615 ns  | 0.4977 ns  | -         |                   `Enum.TryParse<CustomerType>("Standard", out _)` |
| EnumGenerators | 9.067 ns   | 0.0523 ns  | 0.0489 ns  | -         |                   `CustomerTypeExtensions.TryParse("Standard", out _)` |
| SmartEnums     | 30.719 ns  | 0.4043 ns  | 0.3782 ns  | -         |                   `CustomerType.TryFromName( "Gold", out _)` |
| **Intellenums**    | **11.460 ns** | **0.2545 ns** | **0.2380 ns** | **-**         |   **`CustomerType.TryFromName("Standard", out _)`** |

* `Value` (_note that EnumGenerators isn't here as we use the standard C# enum to get its value_)

| Method         | Mean       | Error      | StdDev     | Allocated |
|----------------|-----------|------------|------------|-----------|
| StandardEnums  | 0.0092 ns | 0.0082 ns  | 0.0077 ns  | -         |
| SmartEnums     | 0.3246 ns | 0.0082 ns  | 0.0069 ns  | -         |
| **Intellenums**   | **0.3198 ns** | **0.0103 ns** | **0.0096 ns** | **-**         |

## What does `ToString` return?
It returns the **name** of the member.
There is also a TypeConverter; when this is asked to convert a member to a `string',
it returns the **value** of the member as a string.

## What can the `TypeConverters` convert to and from?
They can convert an underlying type back to a matching enum.

## Can it serialize/deserialize?
Yes, it can. There's various ways to do this, including:
* System.Text.Json
* Newtonsoft.Json
* Dapper
* Entity Framework Core
* Linq2Db
* TypeConverters

Right now, Intellenum serializes using the `Value` property just like native enums.

## I use an Intellenum as a key in a Dictionary - can I serialize that dictionary?
Yes, at least if you use `System.Text.Json`. 

# A look at the generated code
For compile-time constant (and `decimal`) values, a switch expression is generated for `IsDefined`:

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

As an aside, we experimented with using switch expressions for this too, but they turned out to be slower than normal
switch statements (about twice as slow) due to the need for having a tuple in the expression.

The generated code look like this:
```csharp
public static bool TryFromValue(int value, out CustomerType member)
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

A 'compile time constant' is one of the following:
* byte (and unsigned byte)
* int16 (and unsigned int16)
* int32 (and unsigned int32)
* int64 (and unsigned int64)
* string
* decimal

For underlying types that are not one of these, then a lookup table is used.
Why is a lookup table used if the underlying is not one of the above? It is because the left hand side of a switch expression must be 
a constant expression.
A constant expression is used in the 'constant pattern' of the switch expression. The 'constant pattern' is [described as](https://learn.microsoft.com/en-US/dotnet/csharp/language-reference/operators/patterns#constant-pattern):

> A constant pattern is a pattern that matches a constant value. The constant value is specified by a constant expression. A constant expression is an expression that can be fully evaluated at compile time.n a constant pattern, you can use any constant expression, such as:
>
> * an integer or floating-point numerical literal
> * a char
> * a string literal.
> * a Boolean value true or false
> * an enum value
> * the name of a declared const field or local
> * null

So, types like `Guid` and `DateTime` are not allowed on the left hand side of a switch expression (but Span<>'s are).
The alternative in this case is to use a dictionary to map between names and values.


> NOTE: Intellenum is in beta at the moment; I've tested it out and I think it works. The main functionality is present and the API probably 
> won't change significantly from now on. Although it's a fairly new library, it borrows a lot of code and features from [Vogen](https://github.com/SteveDunn/Vogen) which 
> has been in use for a while now by many projects and has lots of downloads, which should provide some confidence.
> Please feel free to try it and provide feedback.

