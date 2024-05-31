// The symbol 'Environment' is banned for use by analyzers: see https://github.com/dotnet/roslyn-analyzers/issues/6467 
#pragma warning disable RS1035

using System;
using System.Text;
using Intellenum.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Intellenum;

internal static class WriteWorkItems
{
    private static readonly ClassGenerator _classGenerator;

    private static readonly string _generatedPreamble = @"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a source generator named Intellenum (https://github.com/SteveDunn/Intellenum)
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

// Suppress warnings about [Obsolete] member usage in generated code.
#pragma warning disable CS0618

// Suppress warnings for 'Override methods on comparable types'.
#pragma warning disable CA1036

// Suppress Error MA0097 : A class that implements IComparable<T> or IComparable should override comparison operators
#pragma warning disable MA0097

// Suppress warning for 'The annotation for nullable reference types should only be used in code within a '#nullable' annotations context. Auto-generated code requires an explicit '#nullable' directive in source.'
// The generator copies signatures from the BCL, e.g. for `TryParse`, and some of those have nullable annotations.
#pragma warning disable CS8669

#pragma warning disable CS1573

// Suppress warnings about CS1591: Missing XML comment for publicly visible type or member 'Type_or_Member'
#pragma warning disable CS1591".Replace("\r\n", "\n").Replace("\n", Environment.NewLine); // normalize regardless of git checkout policy        

    static WriteWorkItems() => _classGenerator = new ClassGenerator();

    public static void WriteVo(VoWorkItem item, SourceProductionContext context, bool isNetFramework)
    {
        // get the recorded user class
        TypeDeclarationSyntax voClass = item.TypeToAugment;

        string classAsText = _generatedPreamble + Environment.NewLine + _classGenerator.BuildClass(item, voClass, isNetFramework);

        SourceText sourceText = SourceText.From(classAsText, Encoding.UTF8);
        
        var unsanitized = $"{item.FullNamespace}_{voClass.Identifier}.g.cs";

        string filename = SanitizeToALegalFilename(unsanitized);

        context.AddSource(filename, sourceText);

        string SanitizeToALegalFilename(string input)
        {
            return input.Replace('@', '_');
        }
    }
}