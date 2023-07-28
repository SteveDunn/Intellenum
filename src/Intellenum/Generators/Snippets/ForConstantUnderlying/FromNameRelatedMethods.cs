using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets.ForConstantUnderlying;

public static class FromNameRelatedMethods
{
    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds)
    {
        var className = tds.Identifier;

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
            {GenerateTryFromNameImplementation(item)}
        }}

        [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
        public static bool IsNamedDefined(ReadOnlySpan<char> name)
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

    public static string GenerateTryFromNameImplementation(VoWorkItem item)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("""
switch (name) 
{
""");

        foreach (var eachMember in item.MemberProperties)
        {
            generate(eachMember.FieldName, eachMember.EnumEnumFriendlyName);
        }

        sb.AppendLine("""
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