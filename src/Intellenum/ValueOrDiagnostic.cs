using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Intellenum;

public class ValueOrDiagnostic<T>
{
    private readonly bool _isValue;

    private ValueOrDiagnostic(T value)
    {
        Value = value;
        _isValue = true;
    }

    private ValueOrDiagnostic(DiagnosticAndLocation diagnostic)
    {
        Diagnostic = diagnostic;
        _isValue = false;
    }

    public static ValueOrDiagnostic<T> WithValue(T value) => new(value);
    
    public static ValueOrDiagnostic<T> WithDiagnostic(DiagnosticAndLocation diagnostic) => new(diagnostic);
    public static ValueOrDiagnostic<T> WithDiagnostic(Diagnostic d, Location l) => WithDiagnostic(new(d, l));

    public bool IsDiagnostic => !_isValue;

    public bool IsValue => _isValue;

    public T Value { get; set; } = default!;

    public DiagnosticAndLocation Diagnostic { get; set; } = null!;
}


public record DiagnosticAndLocation(Diagnostic Diagnostic, Location Location);

public class ValuesOrDiagnostic<T>
{
    private readonly bool _isValue;

    private ValuesOrDiagnostic(IEnumerable<T> values)
    {
        Values = values;
        _isValue = true;
    }

    private ValuesOrDiagnostic(DiagnosticAndLocation diagnosticAndLocation)
    {
        DiagnosticAndLocation = diagnosticAndLocation;
        _isValue = false;
    }

    public static ValuesOrDiagnostic<T> WithValues(IEnumerable<T> value) => new(value);
    public static ValuesOrDiagnostic<T> WithNoValues() => new([]);
    
    
    public static ValuesOrDiagnostic<T> WithDiagnostic(DiagnosticAndLocation diagnosticAndLocation) => new(diagnosticAndLocation);

    public bool IsDiagnostic => !_isValue;

    public bool IsValue => _isValue;

    public IEnumerable<T> Values { get; set; } = default!;

    public DiagnosticAndLocation DiagnosticAndLocation { get; set; } = null!;
}

