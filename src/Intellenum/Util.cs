using System;
using System.Runtime.CompilerServices;
using System.Text;
using Intellenum.Generators.Conversions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

[assembly: InternalsVisibleTo("Intellenum.Tests")]

namespace Intellenum;


public static class Util
{
    static readonly IGenerateConversion[] _conversionGenerators =
    {
        new GenerateSystemTextJsonConversions(),
        new GenerateNewtonsoftJsonConversions(),
        new GenerateTypeConverterConversions(),
        new GenerateDapperConversions(),
        new GenerateEfCoreTypeConversions(),
        new GenerateLinqToDbConversions(),
    };


    public static string GenerateCallToValidateForDeserializing(VoWorkItem workItem)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var eachInstance in workItem.InstanceProperties)
        {
            string escapedName = EscapeIfRequired(eachInstance.FieldName);
            sb.AppendLine($"        if(value == {escapedName}.Value) return {escapedName};");
        }

        return sb.ToString();
    }

    public static string EscapeIfRequired(string name)
    {
        bool match = SyntaxFacts.GetKeywordKind(name) != SyntaxKind.None ||
                     SyntaxFacts.GetContextualKeywordKind(name) != SyntaxKind.None;

        return match ? "@" + name : name;
    }

    public static string GenerateModifiersFor(TypeDeclarationSyntax tds) => string.Join(" ", tds.Modifiers);

    public static string WriteStartNamespace(string @namespace)
    {
        if (string.IsNullOrEmpty(@namespace))
        {
            return string.Empty;
        }

        return @$"namespace {EscapeIfRequired(@namespace)}
{{
";
    }

    public static string WriteCloseNamespace(string @namespace)
    {
        if (string.IsNullOrEmpty(@namespace))
        {
            return string.Empty;
        }

        return @$"}}";
    }

    /// <summary>
    /// These are the attributes that are written to the top of the type, things like
    /// `TypeConverter`, `System.Text.JsonConverter` etc.
    /// </summary>
    /// <param name="tds"></param>
    /// <param name="item"></param>
    /// <returns></returns>
    public static string GenerateAnyConversionAttributes(TypeDeclarationSyntax tds, VoWorkItem item)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var conversionGenerator in _conversionGenerators)
        {
            var attribute = conversionGenerator.GenerateAnyAttributes(tds, item);
            if (!string.IsNullOrEmpty(attribute))
            {
                sb.AppendLine(attribute);
            }
        }

        return sb.ToString();
    }

    public static string GenerateAnyConversionBodies(TypeDeclarationSyntax tds, VoWorkItem item)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var conversionGenerator in _conversionGenerators)
        {
            sb.AppendLine(conversionGenerator.GenerateAnyBody(tds, item));
        }

        return sb.ToString();
    }

    public static string GenerateDebuggerProxyForClasses(TypeDeclarationSyntax tds, VoWorkItem item)
    {
        string code = $@"internal sealed class {item.VoTypeName}DebugView
        {{
            private readonly {item.VoTypeName} _t;

            {item.VoTypeName}DebugView({item.VoTypeName} t)
            {{
                _t = t;
            }}

            public global::System.String UnderlyingType => ""{item.UnderlyingTypeFullName}"";
            public {item.UnderlyingTypeFullName} Value => _t.Value ;

            public global::System.String Conversions => @""{Util.GenerateAnyConversionAttributes(tds, item)}"";
                }}";

        return code;
    }

    public static string GenerateYourAssemblyName() => typeof(Util).Assembly.GetName().Name!;
    public static string GenerateYourAssemblyVersion() => typeof(Util).Assembly.GetName().Version!.ToString();

    public static string GenerateToString(VoWorkItem item) =>
        item.HasToString ? string.Empty
            : $@"/// <summary>Returns the name of the enum.</summary>
    public override global::System.String ToString() => Name;";

    public static string GenerateIComparableImplementationIfNeeded(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        INamedTypeSymbol? primitiveSymbol = item.UnderlyingType;
        if (!primitiveSymbol.ImplementsInterfaceOrBaseClass(typeof(IComparable<>)))
        {
            return string.Empty;
        }
    
        var primitive = tds.Identifier;
        var s = @$"public int CompareTo({primitive} other) => Value.CompareTo(other.Value);
        public int CompareTo(object other) {{
            if(other == null) return 1;
            if(other is {primitive} x) return CompareTo(x);
            throw new global::System.ArgumentException(""Cannot compare to object as it is not of type {primitive}"", nameof(other));
        }}";
    
         return s;
    }

    public static string GenerateDebugAttributes(VoWorkItem item, SyntaxToken className, string itemUnderlyingType)
    {
        var source = $$"""
[global::System.Diagnostics.DebuggerTypeProxyAttribute(typeof({{className}}DebugView))]
    [global::System.Diagnostics.DebuggerDisplayAttribute("Underlying type: {{itemUnderlyingType}}, Value = { _value }")]
""";
        if (item.DebuggerAttributes == DebuggerAttributeGeneration.Basic)
        {
            return $@"/* Debug attributes omitted because the 'debuggerAttributes' flag is set to {nameof(DebuggerAttributeGeneration.Basic)} on the Intellenum attribute.
This is usually set to avoid issues in Rider where it doesn't fully handle the attributes support by Visual Studio and
causes Rider's debugger to crash.

{source}

*/";
        }
    
        return source;
    }

    public static string GenerateIsDefinedBody(VoWorkItem item)
    {
        return item.IsConstant
            ? $"return value switch {{ {generateForConstant()} }};"
            : "return _valuesToEnums.Value.TryGetValue(value, out _);";

        string generateForConstant()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var each in item.InstanceProperties)
            {
                sb.AppendLine($"{each.ValueAsText} => true,");
            }

            sb.AppendLine("_ => false");

            return sb.ToString();
        }
    }

    public static string GenerateFromValueImplementation(VoWorkItem item)
    {
        if (item.IsConstant)
            return $$"""
    bool b = TryFromValue(value, out var ret);
    if(b) return ret;
    throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching instances with a value of '{value}'");
""";
        return $$"""
    bool b =  _valuesToEnums.Value.TryGetValue(value, out var ret);
    if(b) return ret;
    throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching instances with a value of '{value}'");
""";
    }

    public static string GenerateFromNameImplementation(VoWorkItem item) =>
        $$"""
    bool b = TryFromName(name, out var ret);
    if(b) return ret;
    throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching instances named '{name}'");
""";

    public static string GenerateTryFromValueImplementation(VoWorkItem item)
    {
        if (item.IsConstant) return generateFromConstant();

        return "return  _valuesToEnums.Value.TryGetValue(value, out instance);";

        string generateFromConstant()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(
                """
switch (value) 
{
""");
            foreach (var each in item.InstanceProperties)
            {
                // var b = InstanceGeneration.TryBuildInstanceValueAsText(each.Name, each.Value, item.UnderlyingType.FullName());
                // if (!b.Success) throw new InvalidOperationException(b.ErrorMessage);


                generate(each.ValueAsText, each.FieldName);
            }

            sb.AppendLine(
                """
    default:
        instance = default;
        return false;
}
""");

            return sb.ToString();


            void generate(object value, string name)
            {
                sb.AppendLine(
                    $$"""
    case {{value}}:
        instance = {{item.VoTypeName}}.{{name}}; 
        return true;
""");

            }
        }
    }

    public static string GenerateTryFromNameImplementation(VoWorkItem item)
    {
        if (!item.IsConstant) return "return _namesToEnums.Value.TryGetValue(name, out instance);";      
        
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("""
switch (name) 
{
""");

        foreach (var eachInstance in item.InstanceProperties)
        {
            generate(eachInstance.FieldName, eachInstance.EnumEnumFriendlyName);
        }

        sb.AppendLine("""
    default:
        instance = default;
        return false;
}
""");

        return sb.ToString();

        void generate(string fieldName, string name)
            {
                sb.AppendLine(
                    $$"""
    case ("{{name}}"):
        instance = {{item.VoTypeName}}.{{fieldName}}; 
        return true;
""");
            }
    }
    
    public static string GenerateIsNameDefinedImplementation(VoWorkItem item) =>
        """
return TryFromName(name, out _);
""";

    public static string GenerateLazyLookupsIfNeeded(VoWorkItem item)
    {
        if (!item.IsConstant)
        {
            return $$"""
        private static readonly System.Lazy<System.Collections.Generic.Dictionary<string, {{item.UnderlyingTypeFullName}}>> _namesToValues = new( () =>
        new()
        {
            {{ GenerateLazyLookupEntries(item, prop => new($"\"{prop.FieldName}\"", $"{prop.FieldName}.Value")) }}
        });
        private static readonly System.Lazy<System.Collections.Generic.Dictionary<string, {{item.VoTypeName}}>> _namesToEnums = new( () =>
        new()
        {
            {{ GenerateLazyLookupEntries(item, prop => new($"\"{prop.FieldName}\"", $"{prop.FieldName}")) }}
        });
        private static readonly System.Lazy<System.Collections.Generic.Dictionary<{{item.VoTypeName}}, {{item.UnderlyingTypeFullName}}>> _enumsToValues = new( () =>
        new()
        {
            {{ GenerateLazyLookupEntries(item, prop => new($"{prop.FieldName}", $"{prop.FieldName}.Value")) }}
        });
        private static readonly System.Lazy<System.Collections.Generic.Dictionary<{{item.UnderlyingTypeFullName}}, string>> _valuesToNames = new( () =>
        new()
        {
            {{ GenerateLazyLookupEntries(item, prop => new($"{prop.FieldName}.Value", $"\"{prop.FieldName}\"")) }}
        });
        private static readonly System.Lazy<System.Collections.Generic.Dictionary<{{item.UnderlyingTypeFullName}}, {{item.VoTypeName}}>> _valuesToEnums = new( () =>
        new()
        {
            {{ GenerateLazyLookupEntries(item, prop => new($"{prop.FieldName}.Value", $"{prop.FieldName}")) }}
        });
""";
        }

        return string.Empty;
    }

    private static string GenerateLazyLookupEntries(VoWorkItem item, Func<InstanceProperties, (string, string)> callback)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var eachItem in item.InstanceProperties)
        {
            var (first, second) = callback(eachItem);
            sb.AppendLine($"{{ {first}, {second} }},");
        }

        return sb.ToString();
    }

    public static string TryWriteNamespaceIfSpecified(VoWorkItem item)
    {
        if (!string.IsNullOrEmpty(item.UnderlyingType.FullNamespace()))
            return "using " + item.UnderlyingType.FullNamespace() + ";";

        return string.Empty;
    }
}