using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Snippets;

public static class SnippetGenerationFactory
{
    public static string Generate(VoWorkItem item, TypeDeclarationSyntax tds, SnippetType snippetType,
        bool isNetFramework) =>
        (item.IsConstant, snippetType) switch
        {
            (true, SnippetType.FromNameRelatedMethods) => ForConstantUnderlying.FromNameRelatedMethods.Generate(item, tds, isNetFramework),
            (false, SnippetType.FromNameRelatedMethods) => ForNonConstantUnderlying.FromNameRelatedMethods.Generate(item, tds, isNetFramework),
            (true, SnippetType.FromValueRelateMethods) => ForConstantUnderlying.FromValueRelatedMethods.Generate(item, tds),
            (false, SnippetType.FromValueRelateMethods) => ForNonConstantUnderlying.FromValueRelatedMethods.Generate(item, tds),
            _ => throw new ArgumentOutOfRangeException(nameof(snippetType), snippetType, null)
        };
}