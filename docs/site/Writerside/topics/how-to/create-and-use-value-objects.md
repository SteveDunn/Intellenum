# Create and use Intellenum enums

<card-summary>
How to create and use Intellenum enums, how it's easier to create than standard enums, and how more extensible they are than standard enums.
</card-summary>

<note>
This topic is incomplete and is currently being improved.
</note>

Create a new Intellenum enum by decorating a partial class with the `Intellenum` attribute:

```c#
[Intellenum<int>] 
[Member("Standard", 1)]
[Member("Gold", 2)]
public partial class CustomerType;
```

The type must be `partial` as the source generator augments the type with another partial class containing the
generator code.

Now, reference an instance:

```c#
var customerType = CustomerType.Gold;
```

If you try to use the constructors, the [analyzer rules](Analyzer-Rules.md) will catch this and stop you.

As well as accessing instances, you can also quickly find instances, e.g.

```c#
CustomerType.IsDefined("Silver"); // False
```

