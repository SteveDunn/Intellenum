using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Intellenum.Generators.Conversions;

public interface IGenerateConversion
{
    string GenerateAnyAttributes(TypeDeclarationSyntax tds, VoWorkItem item);

    string GenerateAnyBody(TypeDeclarationSyntax tds, VoWorkItem item);
}