# Intellenum - Intelligent Enum Generator

# What is the package?

This is a Source Generator to generate intelligent enums. 

The main goal is that the intelligent enums have almost the same speed and memory performance as using primitive enums and have much faster lookups.

```csharp
[Intellenum<int>]
[Member("One", 1)]
public partial struct Numbers 
{
}
```

You can now lookup names and values in a performant way.

You can also declare instances in different ways, e.g.

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

To see if an enum contains a name, use `IsNameDefined`, or to try to get the named instances, use `TryFromName`.
The same methods exist for values.
