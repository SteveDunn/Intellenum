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

        foreach (var eachMember in workItem.MemberProperties.ValidMembers)
        {
            string escapedName = EscapeIfRequired(eachMember.FieldName);
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
            : $"""
               /// <summary>Returns the name of the enum.</summary>
               public override global::System.String ToString() => _name;
               """;

    public static string GenerateIComparableImplementationIfNeeded(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        StringBuilder sb = new StringBuilder();
        var className = tds.Identifier;

        if (item.IsUnderlyingIsIComparableOfT)
        {
            sb.Append(
                $"""
                 [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                 public int CompareTo({className} other) => Value.CompareTo(other);
                 """);

            AppendOperator("<");
            AppendOperator("<=");
            AppendOperator(">");
            AppendOperator(">=");

            void AppendOperator(string @operator)
            {
                sb.Append(
                    $"""
                     [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                     public static global::System.Boolean operator {@operator}({className} left, {className} right) => left.CompareTo(right) {@operator} 0;
                     """);
            }
        }

        if (item.IsUnderlyingIComparable)
        {
            sb.Append(
                $$"""
                      public int CompareTo(object other) 
                      {
                          if(other is null) return 1;
                          if(other is {{className}} x) return CompareTo(x);
                          throw new global::System.ArgumentException("Cannot compare to object as it is not of type {{className}}", nameof(other));
                      }
                  """);
        }

        return sb.ToString();
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


    public static string GenerateLazyLookupsIfNeeded(VoWorkItem item)
    {
        if (!item.IsConstant)
        {
            return $$"""
        private static readonly System.Lazy<System.Collections.Generic.Dictionary<string, {{item.VoTypeName}}>> _namesToEnums = new( () =>
        new()
        {
            {{ GenerateLazyLookupEntries(item, prop => new($"\"{prop.FieldName}\"", $"{prop.FieldName}")) }}
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

    private static string GenerateLazyLookupEntries(VoWorkItem item, Func<MemberProperties, (string, string)> callback)
    {
        StringBuilder sb = new StringBuilder();

        foreach (var eachItem in item.MemberProperties)
        {
            if (eachItem.IsValue)
            {
                var (first, second) = callback(eachItem.Value);
                sb.AppendLine($"{{ {first}, {second} }},");
            }
        }

        return sb.ToString();
    }

    public static string TryWriteNamespaceIfSpecified(VoWorkItem item)
    {
        var fullNamespace = item.UnderlyingType.FullNamespace();

        // Should ignore using of System namespace as it's provided externally
        if (string.IsNullOrEmpty(fullNamespace) || fullNamespace == "System")
        {
            return string.Empty;
        }

        return $"using {fullNamespace};";
    }
}