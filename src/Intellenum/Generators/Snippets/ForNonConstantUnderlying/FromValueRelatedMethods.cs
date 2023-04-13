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
        /// <returns>The matching enum, or an exception.</returns>
        public static bool TryFromValue({itemUnderlyingType} value, out {className} member)
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
        
    public static string GenerateFromNameRelatedMethods(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        var className = tds.Identifier;

        string s = $@"
        /// <summary>
        /// Gets the matching member based on name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromName(string name)
        {{
            {GenerateFromNameImplementation(item)}
        }}

        /// <summary>
        /// Tries to get the matching member from a name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryFromName(string name, out {className} member)
        {{
            {GenerateTryFromNameImplementation()}
        }}

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsNamedDefined(string name)
        {{
            {GenerateIsNameDefinedImplementation()}
        }}
        ";

        return s;
    }
        
        
    public static string GenerateFromNameImplementation(VoWorkItem item) =>
        $$"""
    bool b = TryFromName(name, out var ret);
    if(b) return ret;
    throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching members named '{name}'");
""";

    private static string GenerateTryFromNameImplementation() => 
        "return _namesToEnums.Value.TryGetValue(name, out member);";

    private static string GenerateIsNameDefinedImplementation() =>
        """
return TryFromName(name, out _);
""";
        
        
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