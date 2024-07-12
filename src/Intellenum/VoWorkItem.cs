using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
#pragma warning disable CS8618

namespace Intellenum;

public class VoWorkItem
{
    private readonly INamedTypeSymbol _underlyingType = null!;
    private readonly string _underlyingTypeFullName = null!;

    public INamedTypeSymbol UnderlyingType
    {
        get => _underlyingType;
        init
        {
            _underlyingType = value;
            _underlyingTypeFullName = value.FullName() ?? value.Name ?? throw new InvalidOperationException(
                "No underlying type specified - please file a bug at https://github.com/SteveDunn/Vogen/issues/new?assignees=&labels=bug&template=BUG_REPORT.yml");
            IsUnderlyingAString = typeof(string).IsAssignableFrom(Type.GetType(_underlyingTypeFullName));
        }
    }
    
    public bool IsUnderlyingAString { get; private set; }

    /// <summary>
    /// The syntax information for the type to augment.
    /// </summary>
    public TypeDeclarationSyntax TypeToAugment { get; init; }
    
    public bool IsValueType { get; init; }

    public required MemberPropertiesCollection MemberProperties { get; init; }

    public  string FullNamespace { get; init; } = string.Empty;

    public Conversions Conversions { get; init; }

    public Customizations Customizations { get; init; }

    public string VoTypeName => TypeToAugment.Identifier.ToString();

    public string UnderlyingTypeFullName =>  UnderlyingType.FullName() ?? UnderlyingType.Name ?? throw new InvalidOperationException(
        "No underlying type specified - please file a bug at https://github.com/SteveDunn/Intellenum/issues/new?assignees=&labels=bug&template=BUG_REPORT.yml");

    public bool HasToString { get; init; }

    public DebuggerAttributeGeneration DebuggerAttributes { get; init; }

    public bool IsConstant { get; init; }
}

public class MemberPropertiesCollection : IEnumerable<ValueOrDiagnostic<MemberProperties>>
{
    private readonly List<ValueOrDiagnostic<MemberProperties>> _items;

    public MemberPropertiesCollection(List<ValueOrDiagnostic<MemberProperties>> items) => _items = items;

    public MemberPropertiesCollection() => _items = new();

    public bool IsFaulty => _items.Any(i => i.IsDiagnostic);
    
    public bool IsEmpty => _items.Count == 0;

    public IEnumerator<ValueOrDiagnostic<MemberProperties>> GetEnumerator() => _items.GetEnumerator();

    public IEnumerable<ValueOrDiagnostic<MemberProperties>> ValidMembers => this.Where(v => v.IsValue);
    public IEnumerable<DiagnosticAndLocation> AllDiagnostics => this.Where(x => x.IsDiagnostic).Select(e => e.Diagnostic);
    
    public IEnumerable<ValueOrDiagnostic<MemberProperties>> ImplicitlyNamedMembers => ValidMembers.Where(v => !v.Value.WasExplicitlyNamed);
    
    public IEnumerable<ValueOrDiagnostic<MemberProperties>> ExplicitlyNamedMembers => ValidMembers.Where(v => v.Value.WasExplicitlyNamed);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerable<string> DescribeAnyDuplicates()
    {
        //find duplicates of values in fromMemberAttributes and report the duplicate value and how many there are
        IEnumerable<MemberProperties> valid = _items.Where(i => i.IsValue).Select(x => x.Value);
        
        var duplicates = valid.GroupBy(a => a.ValueAsText)
            .Where(g => g.Count() > 1);

        
        foreach (IGrouping<string, MemberProperties> duplicate in duplicates)
        {
            var fieldNames = string.Join(", ", duplicate.Select(d => d.FieldName));
            yield return $"The members named: {fieldNames} - repeat the value '{duplicate.Key}'";
        }
    }

    public static MemberPropertiesCollection Empty() => new([]);

    public static MemberPropertiesCollection WithDiagnostic(DiagnosticAndLocation diagnosticAndLocation) => 
        new([ValueOrDiagnostic<MemberProperties>.WithDiagnostic(diagnosticAndLocation)]);

    public static MemberPropertiesCollection WithDiagnostic(Diagnostic d, Location l) => 
        new([ValueOrDiagnostic<MemberProperties>.WithDiagnostic(new DiagnosticAndLocation(d, l))]);

    public void Add(Diagnostic d, Location l)
    {
        _items.Add(ValueOrDiagnostic<MemberProperties>.WithDiagnostic(new DiagnosticAndLocation(d, l)));
    }

    public void Add(MemberProperties memberProperties) => _items.Add(ValueOrDiagnostic<MemberProperties>.WithValue(memberProperties));
    public void Add(ValueOrDiagnostic<MemberProperties> memberProperties) => _items.Add(memberProperties);

    public static MemberPropertiesCollection Combine(params MemberPropertiesCollection[] others) => 
        new(others.SelectMany(o => o).ToList());
}