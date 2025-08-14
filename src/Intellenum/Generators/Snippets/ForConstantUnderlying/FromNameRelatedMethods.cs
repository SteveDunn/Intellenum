using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForConstantUnderlying;

public static class FromNameRelatedMethods
{
    enum ParameterType
    {
        String,
        Span
    }
    
    private const string _parameter = "name";

    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        var className = tds.Identifier;

        string output = GenerateMethods(className, item, ParameterType.String); 

        if (item.SupportsSpans)
        {
            output += GenerateMethods(className, item, ParameterType.Span);
        }

        return output;
    }

//     /// <summary>
//     /// If Spans are supported, then create delegating methods that simply convert the string to a span.
//     /// If not, then generate nothing, as the string version of the methods will be generated anyway.
//     /// </summary>
//     /// <param name="item"></param>
//     /// <param name="className"></param>
//     /// <returns></returns>
//     private static string GenerateDelegatingStringToSpanMethods(VoWorkItem item, SyntaxToken className)
//     {
//         if (item.SupportsSpans)
//         {
//             return "";
//         }
//         
//         return $$"""
//                  #region .NET Framework Compatibility Methods
//                          /// <summary>
//                          /// Gets the matching member based on name.
//                          /// </summary>
//                          /// <param name="{{_parameter}}">The name.</param>
//                          /// <returns>The matching enum, or an exception.</returns>
//                          public static {{className}} FromName(string {{_parameter}})
//                          {
//                              return FromName({{$"{_parameter}.AsSpan()"}});
//                          }
//
//                          /// <summary>
//                          /// Gets the matching member based on name.
//                          /// </summary>
//                          /// <param name="{{_parameter}}">The name.</param>
//                          /// <param name="ignoreCase"><c>true</c> to convert <paramref name="{{_parameter}}"/> in case insensitive mode; <c>false</c> to convert <paramref name="{{_parameter}}"/> in case sensitive mode.</param>
//                          /// <returns>The matching enum, or an exception.</returns>
//                          public static {{className}} FromName(string {{_parameter}}, bool ignoreCase)
//                          {
//                              return FromName({{$"{_parameter}.AsSpan()"}}, ignoreCase);
//                          }
//
//                          /// <summary>
//                          /// Gets the matching member based on name.
//                          /// </summary>
//                          /// <param name="{{_parameter}}">The name.</param>
//                          /// <param name="comparisonType">Comparison type to compare strings for equality during search.</param>
//                          /// <returns>The matching enum, or an exception.</returns>
//                          public static {{className}} FromName(string {{_parameter}}, StringComparison comparisonType)
//                          {
//                              return FromName({{$"{_parameter}.AsSpan()"}}, comparisonType);
//                          }
//
//                          /// <summary>
//                          /// Tries to get the matching member from a name.
//                          /// </summary>
//                          /// <param name="{{_parameter}}">The name.</param>
//                          /// <param name="member">The matching member if successful.</param>
//                          /// <returns>True if found, otherwise false.</returns>
//                          [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
//                          public static bool TryFromName(string {{_parameter}}, out {{className}} member)
//                          {
//                              return TryFromName({{$"{_parameter}.AsSpan()"}}, out member);
//                          }
//
//                          [global::System.ObsoleteAttribute("Please use IsNameDefined rather than this, which has a typo")]
//                          [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
//                          public static bool IsNamedDefined(string {{_parameter}})
//                          {
//                              return IsNamedDefined({{$"{_parameter}.AsSpan()"}});
//                          }
//
//                          [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
//                          public static bool IsNameDefined(string {{_parameter}})
//                          {
//                              return IsNameDefined({{$"{_parameter}.AsSpan()"}});
//                          }
//                  #endregion
//                  """;
//     }

    private static string GenerateMethods(SyntaxToken className, VoWorkItem item, ParameterType pt)
    {
        string parameterType = pt == ParameterType.String ? "System.String" : "System.ReadOnlySpan<char>";
        return $$"""

                         /// <summary>
                         /// Gets the matching member based on name.
                         /// </summary>
                         /// <param name="{{_parameter}}">The name.</param>
                         /// <returns>The matching enum, or an exception.</returns>
                         public static {{className}} FromName({{parameterType}} {{_parameter}})
                         {
                             {{GenerateFromNameImplementation(item, pt)}}
                         }

                         /// <summary>
                         /// Gets the matching member based on name.
                         /// </summary>
                         /// <param name="{{_parameter}}">The name.</param>
                         /// <param name="ignoreCase"><c>true</c> to convert <paramref name="{{_parameter}}"/> in case insensitive mode; <c>false</c> to convert <paramref name="{{_parameter}}"/> in case sensitive mode.</param>
                         /// <returns>The matching enum, or an exception.</returns>
                         public static {{className}} FromName({{parameterType}} {{_parameter}}, bool ignoreCase)
                         {
                             {{ToComparison()}}
                             {{GenerateFromNameImplementation(item, "comparisonType", pt)}}
                         }

                         /// <summary>
                         /// Gets the matching member based on name.
                         /// </summary>
                         /// <param name="{{_parameter}}">The name.</param>
                         /// <param name="comparisonType">Comparison type to compare strings for equality during search.</param>
                         /// <returns>The matching enum, or an exception.</returns>
                         public static {{className}} FromName({{parameterType}} {{_parameter}}, StringComparison comparisonType)
                         {
                             {{GenerateFromNameImplementation(item, "comparisonType", pt)}}
                         }

                         /// <summary>
                         /// Tries to get the matching member from a name.
                         /// </summary>
                         /// <param name="{{_parameter}}">The name.</param>
                         /// <param name="member">The matching member if successful.</param>
                         /// <returns>True if found, otherwise false.</returns>
                         [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                         public static bool TryFromName({{parameterType}} {{_parameter}}, out {{className}} member)
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
                         public static bool TryFromName({{parameterType}} {{_parameter}}, bool ignoreCase, out {{className}} member)
                         {
                             {{ToComparison()}}
                             {{GenerateTryFromNameImplementation(item, "comparisonType", pt)}}
                         }

                         /// <summary>
                         /// Tries to get the matching member from a name.
                         /// </summary>
                         /// <param name="{{_parameter}}">The name.</param>
                         /// <param name="member">The matching member if successful.</param>
                         /// <returns>True if found, otherwise false.</returns>
                         [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                         public static bool TryFromName({{parameterType}} {{_parameter}}, StringComparison comparisonType, out {{className}} member)
                         {
                             {{GenerateTryFromNameImplementation(item, "comparisonType", pt)}}
                         }

                         [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                         public static bool IsNameDefined({{parameterType}} {{_parameter}})
                         {
                             {{GenerateIsNameDefinedImplementation()}}
                         }

                 """;
    }

    private static string GenerateFromNameImplementation(VoWorkItem item, ParameterType parameterType)
    {
        string toString = parameterType == ParameterType.Span ? ".ToString()" : "";
        return $$"""
                     if (TryFromName({{_parameter}}, out var ret)) 
                     {
                         return ret;
                     }
                     
                     ThrowHelper.ThrowMatchFailed($"{{item.VoTypeName}} has no matching members named '{{{_parameter}}{{toString}}}'");
                     
                     return default;
                 """;
    }

    private static string GenerateFromNameImplementation(VoWorkItem item, string comp, ParameterType parameterType)
    {
        string toString = parameterType == ParameterType.Span ? ".ToString()" : "";

        return $$"""
                     if (TryFromName({{_parameter}}, {{comp}}, out var ret)) 
                     {
                         return ret;
                     }
                     
                     ThrowHelper.ThrowMatchFailed($"{{item.VoTypeName}} has no matching members named '{{{_parameter}}{{toString}}}'");
                     
                     return default;
                 """;
    }

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

    private static string GenerateTryFromNameImplementation(VoWorkItem item, string comp, ParameterType pt)
    {
        var sb = new StringBuilder();
        var typeName = item.VoTypeName;

        foreach (var eachMember in item.MemberProperties)
        {
            var value = eachMember.Value;
            sb.AppendLine(
                pt == ParameterType.String
                    ? $$"""
                        if (string.Equals({{_parameter}}, "{{value.EnumEnumFriendlyName}}", {{comp}}))
                        {
                            member = {{typeName}}.{{value.FieldName}}; 
                            return true;
                        }

                        """
                    : $$"""
                        if (MemoryExtensions.Equals({{_parameter}}, "{{value.EnumEnumFriendlyName}}".AsSpan(), {{comp}}))
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


    private static string ToComparison() =>
        "StringComparison comparisonType = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;";

    private static string GenerateIsNameDefinedImplementation() =>
        $"""
         return TryFromName({_parameter}, out _);
         """;
}