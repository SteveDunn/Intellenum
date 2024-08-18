using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForConstantUnderlying;

public static class FromNameRelatedMethods
{
    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds, bool isNetFramework)
    {
        var className = tds.Identifier;

        string netFrameworkCompatabilityMethods =
            $$"""

              #region .NET Framework Compatability Methods
              
                      /// <summary>
                      /// Gets the matching member based on name.
                      /// </summary>
                      /// <param name="name">The name.</param>
                      /// <returns>The matching enum, or an exception.</returns>
                      public static {{className}} FromName(string name)
                      {
                          return FromName(name.AsSpan());
                      }
              
                      /// <summary>
                      /// Tries to get the matching member from a name.
                      /// </summary>
                      /// <param name="name">The name.</param>
                      /// <param name="member">The matching member if successful.</param>
                      /// <returns>True if found, otherwise false.</returns>
                      [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                      public static bool TryFromName(string name, out {{className}} member)
                      {
                          return TryFromName(name.AsSpan(), out member);
                      }
              
                      [global::System.ObsoleteAttribute("Please use IsNameDefined rather than this, which has a typo")]
                      [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                      public static bool IsNamedDefined(string name)
                      {
                          return IsNamedDefined(name.AsSpan());
                      }
              
                      [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                      public static bool IsNameDefined(string name)
                      {
                          return IsNameDefined(name.AsSpan());
                      }

              #endregion
                      
                      
              """;

        string s =
            $$"""
              
                      /// <summary>
                      /// Gets the matching member based on name.
                      /// </summary>
                      /// <param name="name">The name.</param>
                      /// <returns>The matching enum, or an exception.</returns>
                      public static {{className}} FromName(ReadOnlySpan<char> name)
                      {
                          {{GenerateFromNameImplementation(item)}}
                      }
              
                      /// <summary>
                      /// Tries to get the matching member from a name.
                      /// </summary>
                      /// <param name="name">The name.</param>
                      /// <param name="member">The matching member if successful.</param>
                      /// <returns>True if found, otherwise false.</returns>
                      [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                      public static bool TryFromName(ReadOnlySpan<char> name, out {{className}} member)
                      {
                          {{GenerateTryFromNameImplementation(item)}}
                      }
              
                      [global::System.ObsoleteAttribute("Please use IsNameDefined rather than this, which has a typo")]
                      [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                      public static bool IsNamedDefined(ReadOnlySpan<char> name)
                      {
                          {{GenerateIsNameDefinedImplementation()}}
                      }
              
                      [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                      public static bool IsNameDefined(ReadOnlySpan<char> name)
                      {
                          {{GenerateIsNameDefinedImplementation()}}
                      }
                      
              """;

        return isNetFramework ? netFrameworkCompatabilityMethods + s : s;
    }


    public static string GenerateFromNameImplementation(VoWorkItem item) =>
        $$"""
              bool b = TryFromName(name, out var ret);
              if(b) 
              {
                  return ret;
              }
              
              ThrowHelper.ThrowMatchFailed($"{{item.VoTypeName}} has no matching members named '{name.ToString()}'");
              
              return default;
          """;

    public static string GenerateTryFromNameImplementation(VoWorkItem item)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(
            """
            switch (name) 
            {
            """);

        foreach (var eachMember in item.MemberProperties)
        {
            generate(eachMember.Value.FieldName, eachMember.Value.EnumEnumFriendlyName);
        }

        sb.AppendLine(
            """
                default:
                    member = default;
                    return false;
            }
            """);

        return sb.ToString();

        void generate(string fieldName, string name)
        {
            sb.AppendLine(
                $$"""
                      case ("{{name}}"):
                          member = {{item.VoTypeName}}.{{fieldName}}; 
                          return true;
                  """);
        }
    }

    private static string GenerateIsNameDefinedImplementation() =>
        """
        return TryFromName(name, out _);
        """;
}