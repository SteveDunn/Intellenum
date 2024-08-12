using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Intellenum;

#pragma warning disable CS8618
public class MemberPropertiesCollection : IEnumerable<ValueOrDiagnostic<MemberProperties>>
{
    private readonly List<ValueOrDiagnostic<MemberProperties>> _items;

    public MemberPropertiesCollection(List<ValueOrDiagnostic<MemberProperties>> items) => _items = items;

    public MemberPropertiesCollection() => _items = new();

    public bool IsEmpty => _items.Count == 0;

    public IEnumerator<ValueOrDiagnostic<MemberProperties>> GetEnumerator() => _items.GetEnumerator();

    public IEnumerable<MemberProperties> ValidMembers => this.Where(v => v.IsValue).Select(x => x.Value);
    public IEnumerable<DiagnosticAndLocation> AllDiagnostics => this.Where(x => x.IsDiagnostic).Select(e => e.Diagnostic);
    
    public IEnumerable<MemberProperties> MembersThatNeedInitializing => ValidMembers.Where(v => v.NeedsInitializing);
    
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

    public bool NeedsStaticConstructor => ValidMembers.Any(m => m.NeedsInitializing);

    public static MemberPropertiesCollection Empty() => new([]);

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