# Tips for working with EFCore

<note>
This topic is copied from Vogen and/or is incomplete. It is being worked on (or is planned
to be worked on). 

If you would like to help with this, please see the list of [open issues](https://github.com/SteveDunn/Intellenum/issues).
</note>


<toc></toc>

## Identifying types that are generated by Vogen

Thank you to [@jeffward01](https://github.com/jeffward01) for this item.

The goal of this is to identify types that are generated by Vogen.

### Use Case
I use Vogen alongside EfCore, I like to programmatically add ValueConverters by convention, I need to identity which properties on my entities are Vogen Generated value objects.

### Solution
Vogen decorates the source it generates with the `GeneratedCodeAttribute`. This provides metadata about the tool which generated the code, this is what we'll use as an identifier.

Note: the code snippets use:

* [CSharpFunctionalExtensions](https://github.com/vkhorikov/CSharpFunctionalExtensions) - to use `Maybe<T>`
BTW - if you're reading this, and have not checked out this library, I highly recommend.
You don't need to adopt the pattern 100%, treat it as a buffet and take / use what you want

* [FluentAssertions—](https://github.com/fluentassertions/fluentassertions)in the unit tests, just because

* XUnit, because it's better than NUnit and MSTest 🙃

Code Snippet

```c#
// Helper class
internal static class AttributeHelper
{
    public static bool IsVogenValueObject(this Type targetType)
    {
        Maybe<GeneratedCodeAttribute> generatedCodeAttribute = targetType.GetClassAttribute<GeneratedCodeAttribute>();
        return generatedCodeAttribute.HasValue && generatedCodeAttribute.Value.Tool == "Vogen";
    }

    private static Maybe<TAttribute> GetClassAttribute<TAttribute>(this Type targetType)
        where TAttribute : Attribute
    {
        return targetType.GetAttribute<TAttribute>();
    }
}
```

Usage Example (From EfCore)

```c#
 foreach (IMutableEntityType entityType in builder.Model.GetEntityTypes())
        {
            PropertyInfo[] properties = entityType.ClrType.GetProperties();

            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.PropertyType.IsVogenValueObject())
                {
                     // Huzzah!
                     // Do something with the property that is a value object generated by Vogen....
                }
            }
        }

```

### Testing

Data for unit test

```c#
[ValueObject<Guid>]
// ReSharper disable once PartialTypeWithSinglePart
public readonly partial struct VogenStronglyTypedId {}

Unit Test

```c#
public class VogenStronglyTypedIdTests
{
    [Fact]
    public void ShouldIdentityVogenAttributeByHelperMethod()
    {
        Type vogenType = typeof(VogenStronglyTypedId);

        Maybe<GeneratedCodeAttribute> generatedCodeAttribute = vogenType.GetClassAttribute<GeneratedCodeAttribute>();
        Assert.True(generatedCodeAttribute.HasValue);
        GeneratedCodeAttribute? valueOfAttribute = generatedCodeAttribute.Value;
        valueOfAttribute.Tool.Should()
            .Be("Vogen");
    }
    
    [Fact]
    public void ShouldIdentityVogenAttribute()
    {
        Type vogenType = typeof(VogenStronglyTypedId);
        vogenType.IsVogenValueObject()
            .Should()
            .Be(true);
    }
}
```

### Summary

Jeff says:
<note>
I think this is outside the scope of the library, but I imagine a large majority of Vogen users have some use-case where this will be helpful.
</note>

## Adding all value objects to the `ModelConfigurationBuilder`

Thank you to [CheloXL](https://github.com/CheloXL) for this tip

Just to add another snippet that I'm using in my solutions:
Add all VOs that have EfCoreValueConverter to the `ModelConfigurationBuilder`:

```C#
internal static class VogenExtensions
{
    public static void ApplyVogenEfConvertersFromAssembly(this ModelConfigurationBuilder configurationBuilder, Assembly assembly)
    {
        var types = assembly.GetTypes();

		foreach (var type in types)
		{
			if (IsVogenValueObject(type) && TryGetEfValueConverter(type, out var efCoreConverterType))
			{
				configurationBuilder.Properties(type).HaveConversion(efCoreConverterType);
			}
		}
	}

	private static bool TryGetEfValueConverter(Type type, [NotNullWhen(true)]out Type? efCoreConverterType)
	{
		var inner = type.GetNestedTypes();

		foreach (var innerType in inner)
		{
			if (!typeof(ValueConverter).IsAssignableFrom(innerType) || !"EfCoreValueConverter".Equals(innerType.Name, StringComparison.Ordinal))
			{
				continue;
			}

			efCoreConverterType = innerType;
			return true;
		}

		efCoreConverterType = null;
		return false;
	}

	private static bool IsVogenValueObject(MemberInfo targetType)
	{
		var generatedCodeAttribute =targetType.GetCustomAttribute<GeneratedCodeAttribute>();
		return "Vogen".Equals(generatedCodeAttribute?.Tool, StringComparison.Ordinal);
	}
}
```

Usage: In your `dbcontext`:
```C#
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    configurationBuilder.ApplyVogenEfConvertersFromAssembly(typeof(YOURDBCONTEXT).Assembly);
}
```

