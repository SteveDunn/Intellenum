using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForConstantUnderlying;

public static class FromNameRelatedMethods
{
    private const string _parameter = "name";

    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds, bool isNetFramework)
    {
        var className = tds.Identifier;

        var nameMethods = GenerateNameMethods(className, item);
        
        return isNetFramework ? GenerateFrameworkNameMethods(className) + nameMethods : nameMethods;
    }

    private static string GenerateFrameworkNameMethods(SyntaxToken className) =>
        $$"""

          #region .NET Framework Compatibility Methods
                  /// <summary>
                  /// Gets the matching member based on name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <returns>The matching enum, or an exception.</returns>
                  public static {{className}} FromName(string {{_parameter}})
                  {
                      return FromName({{ConvertFromFramework()}});
                  }

                  /// <summary>
                  /// Gets the matching member based on name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="ignoreCase"><c>true</c> to convert <paramref name="{{_parameter}}"/> in case insensitive mode; <c>false</c> to convert <paramref name="{{_parameter}}"/> in case sensitive mode.</param>
                  /// <returns>The matching enum, or an exception.</returns>
                  public static {{className}} FromName(string {{_parameter}}, bool ignoreCase)
                  {
                      return FromName({{ConvertFromFramework()}}, ignoreCase);
                  }

                  /// <summary>
                  /// Gets the matching member based on name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="comparisonType">Comparison type to compare strings for equality during search.</param>
                  /// <returns>The matching enum, or an exception.</returns>
                  public static {{className}} FromName(string {{_parameter}}, StringComparison comparisonType)
                  {
                      return FromName({{ConvertFromFramework()}}, comparisonType);
                  }

                  /// <summary>
                  /// Tries to get the matching member from a name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="member">The matching member if successful.</param>
                  /// <returns>True if found, otherwise false.</returns>
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool TryFromName(string {{_parameter}}, out {{className}} member)
                  {
                      return TryFromName({{ConvertFromFramework()}}, out member);
                  }
          
                  [global::System.ObsoleteAttribute("Please use IsNameDefined rather than this, which has a typo")]
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool IsNamedDefined(string {{_parameter}})
                  {
                      return IsNamedDefined({{ConvertFromFramework()}});
                  }
          
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool IsNameDefined(string {{_parameter}})
                  {
                      return IsNameDefined({{ConvertFromFramework()}});
                  }
          #endregion
          """;

    private static string GenerateNameMethods(SyntaxToken className, VoWorkItem item) =>
        $$"""
          
                  /// <summary>
                  /// Gets the matching member based on name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <returns>The matching enum, or an exception.</returns>
                  public static {{className}} FromName(ReadOnlySpan<char> {{_parameter}})
                  {
                      {{GenerateFromNameImplementation(item)}}
                  }

                  /// <summary>
                  /// Gets the matching member based on name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="ignoreCase"><c>true</c> to convert <paramref name="{{_parameter}}"/> in case insensitive mode; <c>false</c> to convert <paramref name="{{_parameter}}"/> in case sensitive mode.</param>
                  /// <returns>The matching enum, or an exception.</returns>
                  public static {{className}} FromName(ReadOnlySpan<char> {{_parameter}}, bool ignoreCase)
                  {
                      {{ToComparison()}}
                      {{GenerateFromNameImplementation(item, "comparisonType")}}
                  }

                  /// <summary>
                  /// Gets the matching member based on name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="comparisonType">Comparison type to compare strings for equality during search.</param>
                  /// <returns>The matching enum, or an exception.</returns>
                  public static {{className}} FromName(ReadOnlySpan<char> {{_parameter}}, StringComparison comparisonType)
                  {
                      {{GenerateFromNameImplementation(item, "comparisonType")}}
                  }

                  /// <summary>
                  /// Tries to get the matching member from a name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="member">The matching member if successful.</param>
                  /// <returns>True if found, otherwise false.</returns>
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool TryFromName(ReadOnlySpan<char> {{_parameter}}, out {{className}} member)
                  {
                      {{GenerateTryFromNameImplementation(item)}}
                  }

                  /// <summary>
                  /// Tries to get the matching member from a name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="member">The matching member if successful.</param>
                  /// <returns>True if found, otherwise false.</returns>
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool TryFromName(ReadOnlySpan<char> {{_parameter}}, bool ignoreCase, out {{className}} member)
                  {
                      {{ToComparison()}}
                      {{GenerateTryFromNameImplementation(item, "comparisonType")}}
                  }

                  /// <summary>
                  /// Tries to get the matching member from a name.
                  /// </summary>
                  /// <param name="{{_parameter}}">The name.</param>
                  /// <param name="member">The matching member if successful.</param>
                  /// <returns>True if found, otherwise false.</returns>
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool TryFromName(ReadOnlySpan<char> {{_parameter}}, StringComparison comparisonType, out {{className}} member)
                  {
                      {{GenerateTryFromNameImplementation(item, "comparisonType")}}
                  }

                  [global::System.ObsoleteAttribute("Please use IsNameDefined rather than this, which has a typo")]
                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool IsNamedDefined(ReadOnlySpan<char> {{_parameter}})
                  {
                      {{GenerateIsNameDefinedImplementation()}}
                  }

                  [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                  public static bool IsNameDefined(ReadOnlySpan<char> {{_parameter}})
                  {
                      {{GenerateIsNameDefinedImplementation()}}
                  }

          """;

    private static string GenerateFromNameImplementation(VoWorkItem item) =>
        $$"""
              if (TryFromName({{_parameter}}, out var ret)) 
              {
                  return ret;
              }
              
              ThrowHelper.ThrowMatchFailed($"{{item.VoTypeName}} has no matching members named '{{{_parameter}}}'");
              
              return default;
          """;

    private static string GenerateFromNameImplementation(VoWorkItem item, string comp) =>
        $$"""
              if (TryFromName({{_parameter}}, {{comp}}, out var ret)) 
              {
                  return ret;
              }
              
              ThrowHelper.ThrowMatchFailed($"{{item.VoTypeName}} has no matching members named '{{{_parameter}}}'");
              
              return default;
          """;

    private static string GenerateTryFromNameImplementation(VoWorkItem item)
    {
        var sb = new StringBuilder();
        sb.AppendLine(
            $$"""
              switch ({{_parameter}}) 
              {
              """);

        foreach (var eachMember in item.MemberProperties)
        {
            GenerateCase(eachMember.Value.EnumEnumFriendlyName, item.VoTypeName, eachMember.Value.FieldName);
        }

        sb.AppendLine(
            """
                default:
                    member = default;
                    return false;
            }
            """);

        return sb.ToString();

        void GenerateCase(string name, string typeName, string fieldName)
        {
            sb.AppendLine(
                $"""
                      case ("{name}"):
                          member = {typeName}.{fieldName}; 
                          return true;
                  """);
        }
    }

    private static string GenerateTryFromNameImplementation(VoWorkItem item, string comp)
    {
        var sb = new StringBuilder();
        var typeName = item.VoTypeName;
        foreach (var eachMember in item.MemberProperties)
        {
            var value = eachMember.Value;
            sb.AppendLine(
            $$"""
            if (MemoryExtensions.Equals({{_parameter}}, {{value.EnumEnumFriendlyName}}.Name, {{comp}}))
            {
                member = {{typeName}}.{{value.FieldName}}; 
                return true;
            }

            """);
        }
        sb.AppendLine(
            """
            member = default;
            return false;
            """);
        return sb.ToString();
    }


    private static string ConvertFromFramework() => $"{_parameter}.AsSpan()";

    private static string ToComparison() =>
        "StringComparison comparisonType = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;";

    private static string GenerateIsNameDefinedImplementation() =>
        $"""
         return TryFromName({_parameter}, out _);
         """;
}