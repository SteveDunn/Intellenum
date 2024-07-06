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

    private ValueOrDiagnostic(Diagnostic diagnostic)
    {
        Diagnostic = diagnostic;
        _isValue = false;
    }

    public static ValueOrDiagnostic<T> WithValue(T value) => new(value);
    
    public static ValueOrDiagnostic<T> WithDiagnostic(Diagnostic diagnostic) => new(diagnostic);

    public bool IsDiagnostic => !_isValue;

    public bool IsValue => _isValue;

    public T Value { get; set; } = default!;

    public Diagnostic Diagnostic { get; set; } = null!;
}


public class ValuesOrDiagnostic<T>
{
    private readonly bool _isValue;

    private ValuesOrDiagnostic(IEnumerable<T> values)
    {
        Values = values;
        _isValue = true;
    }

    private ValuesOrDiagnostic(Diagnostic diagnostic)
    {
        Diagnostic = diagnostic;
        _isValue = false;
    }

    public static ValuesOrDiagnostic<T> WithValue(IEnumerable<T> value) => new(value);
    public static ValuesOrDiagnostic<T> WithNoValues() => new([]);
    
    
    public static ValuesOrDiagnostic<T> WithDiagnostic(Diagnostic diagnostic) => new(diagnostic);

    public bool IsDiagnostic => !_isValue;

    public bool IsValue => _isValue;

    public IEnumerable<T> Values { get; set; } = default!;

    public Diagnostic Diagnostic { get; set; } = null!;
}

