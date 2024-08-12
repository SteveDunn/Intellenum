# Create and use Intellenum enums

<card-summary>
How to create and use Intellenum enums, how it's easier to create than standard enums, and how more extensible they are than standard enums.
</card-summary>

The tutorial on ["your first enum"](your-first-intellenum-enum.md)
introduced you to some of the different ways to create an Intellenum.

In this how-to article, we'll explore the rest. The different ways to create instances are:

* Field declarations
* the `Member` attribute
* the `Members` (plural) attribute
* The `Member` method
* The `Members` (plural) method

Let's look at each way:

## Field declarations

```c#
[Intellenum<string>] 
public partial class CustomerType
{
    public static readonly CustomerType Standard, Silver, Gold;
}
```

<note>
the partial keyword is required as the code generator augments this type by creating another partial class
</note>

This produces an instance with three members.

```C#
Console.WriteLine(CustomerType.Standard); // "Standard"
Console.WriteLine(CustomerType.Silver); // "Silver"
Console.WriteLine(CustomerType.Gold); // "Gold"
```

You can also use different values, e.g.
```C#
[Intellenum<string>]
public partial class CustomerType
{
    public static readonly CustomerType 
        Standard = new("STD"), 
        Silver = new("SLVR"), 
        Gold = new ("GLD");
}
```

The above examples are for enums based on `string`. The **values** are defaulted to be the same as **name**. 

For `int`-based enums, values are defaulted start at zero and increment by one:

```c#
[Intellenum<int>]
public partial class CustomerType
{
    public static readonly CustomerType Standard, Silver, Gold;
}
```

This produces an instance with three members.

```C#
Console.WriteLine(CustomerType.Standard); // 0
Console.WriteLine(CustomerType.Silver); // 1
Console.WriteLine(CustomerType.Gold); // 2
```

### Attributes

You can use attributes to declare members, e.g.

```c#
[Intellenum<string>]
[Members("Standard, Gold, Silver")]
public partial class CustomerType;
```

... or individually
```c#
[Intellenum<string>]
[Member("Standard")]
[Member("Gold")]
[Member("Silver")]
public partial class CustomerType;
```

### Static constructors

You can declare members via the `Members` and `Member` methods from a static constructor:

```c#
[Intellenum<string>
public partial class CustomerType
{
    static CustomerType() => Members("Standard, Silver, Gold");
}
```



