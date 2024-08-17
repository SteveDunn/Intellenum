using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForConstantUnderlying;

public static class FromValueRelatedMethods
{
    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        var className = tds.Identifier;

        var itemUnderlyingType = item.UnderlyingTypeFullName;

        string s =
            $$"""
              
                      /// <summary>
                      /// Builds a member from an enum value.
                      /// </summary>
                      /// <param name="value">The value.</param>
                      /// <returns>The matching enum, or an exception.</returns>
                      public static {{className}} FromValue({{itemUnderlyingType}} value)
                      {
                          {{GenerateFromValueImplementation(item)}}
                      }            
              
                      /// <summary>
                      /// Tries to get a member based on value.
                      /// </summary>
                      /// <param name="value">The value.</param>
                      /// <param name="member">The matching member if successful.</param>
                      /// <returns>The matching enum, or an exception.</returns>
                      public static bool TryFromValue({{itemUnderlyingType}} value, {{SnippetGenerationFactory.GenerateNotNullWhenAttribute()}} out {{className}} member)
                      {
                          {{GenerateTryFromValueImplementation(item)}}
                      }        
              
                      /// <summary>
                      /// Determines if there is a member that is defined with the specified value.
                      /// </summary>
                      /// <param name="value">The value to search for.</param>
                      /// <returns>True if there is a member with matching value, otherwise False.</returns>
                      public static bool IsDefined({{itemUnderlyingType}} value)
                      {
                          {{GenerateIsDefinedBody(item)}}
                      }
                      
              """;

        return s;
    }


    private static string GenerateFromValueImplementation(VoWorkItem item) =>
        $$"""
              bool b = TryFromValue(value, out var ret);
              if(b) 
              {
                return ret;
              }
              throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching members with a value of '{value}'");
          """;

    private static string GenerateIsDefinedBody(VoWorkItem item)
    {
        return $$"""
                 return value switch 
                 {
                     {{GenerateForConstant()}} 
                 };
                 """;

        string GenerateForConstant()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var each in item.MemberProperties)
            {
                sb.AppendLine($"{each.Value.ValueAsText} => true,");
            }

            sb.AppendLine("_ => false");

            return sb.ToString();
        }
    }


    private static string GenerateTryFromValueImplementation(VoWorkItem item)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(
            """
            switch (value) 
            {
            """);
        foreach (var each in item.MemberProperties)
        {
            GenerateCase(each.Value.ValueAsText, each.Value.FieldName);
        }

        sb.AppendLine(
            """
                default:
                    member = default;
                    return false;
            }
            """);

        return sb.ToString();


        void GenerateCase(object value, string name)
        {
            sb.AppendLine(
                $"""
                     case {value}:
                         member = {item.VoTypeName}.{name}; 
                         return true;
                 """);

        }
    }
    
}