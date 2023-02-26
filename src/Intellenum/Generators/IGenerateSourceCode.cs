using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators;

public interface IGenerateSourceCode
{
    string BuildClass(VoWorkItem item, TypeDeclarationSyntax tds);
}