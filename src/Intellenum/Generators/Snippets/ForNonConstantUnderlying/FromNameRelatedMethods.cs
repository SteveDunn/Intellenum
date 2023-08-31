using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForNonConstantUnderlying;

public static class FromNameRelatedMethods
{
    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds, bool isNetFramework)
    {
        var className = tds.Identifier;

        string netFrameworkCompatabilityMethods = $@"
#region .NET Framework Compatability Methods

        /// <summary>
        /// Gets the matching member based on name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromName(string name)
        {{
            return FromName(name.AsSpan());
        }}

        /// <summary>
        /// Tries to get the matching member from a name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryFromName(string name, out {className} member)
        {{
            return TryFromName(name.AsSpan(), out member);
        }}

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsNamedDefined(string name)
        {{
            return IsNamedDefined(name.AsSpan());
        }}

#endregion
        
        ";

        string s = $@"
        /// <summary>
        /// Gets the matching member based on name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        public static {className} FromName(ReadOnlySpan<char> name)
        {{
            {GenerateFromNameImplementation(item)}
        }}

        /// <summary>
        /// Tries to get the matching member from a name.
        /// </summary>
        /// <param name=""name"">The name.</param>
        /// <returns>The matching enum, or an exception.</returns>
        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool TryFromName(ReadOnlySpan<char> name, out {className} member)
        {{
            {GenerateTryFromNameImplementation(className)}
        }}

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsNamedDefined(ReadOnlySpan<char> name)
        {{
            {GenerateIsNameDefinedImplementation()}
        }}
        ";

        return isNetFramework ? netFrameworkCompatabilityMethods + s : s;
    }
        
        
    public static string GenerateFromNameImplementation(VoWorkItem item) =>
        $$"""
    bool b = TryFromName(name, out var ret);
    if(b) return ret;
    throw new {{nameof(IntellenumMatchFailedException)}}($"{{item.VoTypeName}} has no matching members named '{name.ToString()}'");
""";

    private static string GenerateTryFromNameImplementation(SyntaxToken className) =>
        $$"""
        // Not a fan of using foreach here, but Dictionary<,>.KeyCollection is a bit limited
        foreach (var key in _namesToEnums.Value.Keys)
        {
            if (key.AsSpan() != name)
            {
                continue;
            }
            
            member = _namesToEnums.Value[key];
            return true;
        }
        
        member = default({{className}});
        return false;
        """;

    private static string GenerateIsNameDefinedImplementation() =>
        """
return TryFromName(name, out _);
""";
}