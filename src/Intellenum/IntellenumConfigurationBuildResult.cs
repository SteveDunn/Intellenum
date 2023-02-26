using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Intellenum;

internal sealed class IntellenumConfigurationBuildResult
{
    public IntellenumConfiguration? ResultingConfiguration { get; set; }

    public List<Diagnostic> Diagnostics { get; set; } = new();

    public static IntellenumConfigurationBuildResult Null => new();

    public void AddDiagnostic(Diagnostic diagnostic) => Diagnostics.Add(diagnostic);
}