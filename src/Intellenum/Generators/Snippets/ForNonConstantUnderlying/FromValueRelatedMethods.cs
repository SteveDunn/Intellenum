using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForNonConstantUnderlying;

public static class FromValueRelatedMethods
{
    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        var className = tds.Identifier;

        var itemUnderlyingType = item.UnderlyingTypeFullName;

        string s = $@"
        /// <summary>
        /// Builds a member from an enum value.
        /// </summary>
        /// <param name=""value"">The value.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromValue({itemUnderlyingType} value)
        {{
            {GenerateFromValueImplementation(item)}
        }}            

        /// <summary>
        /// Tries to get a member based on value.
        /// </summary>
        /// <param name=""value"">The value.</param>
        /// <param name=""member"">The matching member if successful.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static bool TryFromValue({itemUnderlyingType} value, {SnippetGenerationFactory.GenerateNotNullWhenAttribute()} out {className} member)
        {{
            {GenerateTryFromValueImplementation()}
        }}        

        /// <summary>
        /// Determines if there is a member that is defined with the specified value.
        /// </summary>
        /// <param name=""value"">The value to search for.</param>
        /// <returns>True if there is a member with matching value, otherwise False.</returns>
        public static bool IsDefined({itemUnderlyingType} value)
        {{
            {GenerateIsDefinedBody()}
        }}
        ";

        return s;
    }


    private static string GenerateIsDefinedBody() => "return _valuesToEnums.Value.TryGetValue(value, out _);";


    private static string GenerateFromValueImplementation(VoWorkItem item) =>
        $$"""
              bool b =  _valuesToEnums.Value.TryGetValue(value, out var ret);
              if(b) return ret;
              throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching members with a value of '{value}'");
          """;

    private static string GenerateTryFromValueImplementation() => 
        "return  _valuesToEnums.Value.TryGetValue(value, out member);";
}