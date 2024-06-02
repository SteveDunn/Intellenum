# Your first Intellenum instance

<card-summary>
Create and use your first Intellenum instance
</card-summary>

In this tutorial, we'll create and use an instance of an Intellenum.

[Install](Installation.md) the package, and then, in your project, create a new Intellenum that represents a customer type:

```C#
[Intellenum<int>]
[Member("Standard", 1)]
[Member("Gold", 2)]
public partial class CustomerType;
```

If you're not using generics, you can use `typeof` instead:

```c#
[Intellenum(typeof(int))]
[Member("Standard", 1)]
[Member("Gold", 2)]
public partial class CustomerType;
```

<note>
the partial keyword is required as the code generator augments this type by creating another partial class
</note>

Now, write an instance:

```c#
Console.WriteLine(CustomerType.Gold.Name);  // Gold
Console.WriteLine(CustomerType.Gold.Value); // 2
```

If you try to use a constructor, then the [analyzer rules](Analyzer-Rules.md) will catch this and stop you.

![analysis-error-when-newing-up.png](analysis-error-when-newing-up.png)

You can now perform fast lookups by name or value:

```c#
var find1 = CustomerType.FromName("Gold");
var find2 = CustomerType.FromValue(2);
```

You can also list values:

```c#
Console.WriteLine(
    string.Join(", ", CustomerType.List())); // Standard, Gold
```

To see if a value is defined, use:

```c#
Console.WriteLine(CustomerType.IsNamedDefined("Silver")); // False
```

... or, to see if something is defined by value:

```c#
Console.WriteLine(CustomerType.IsDefined(3)); // False
```

To try to get instances from name or value:

```c#
bool b1 = CustomerType.TryFromName(
    "Standard", out CustomerType standard);

bool b2 = CustomerType.TryFromValue(
    2, out CustomerType gold);

bool b3 = CustomerType.TryFromName(
    "Silver", out CustomerType silver);

Console.WriteLine(b1 + " " + standard.Name);
Console.WriteLine(b2 + " " + gold.Name);
Console.WriteLine(b3);
```

