using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace Intellenum;

public class MemberProperties
{
    public MemberProperties(MemberSource source,
        string fieldName,
        string enumFriendlyName,
        string valueAsText,
        object value,
        string tripleSlashComments,
        bool wasExplicitlySetAName,
        bool wasExplicitlySetAValue)
    {
        Source = source;
        FieldName = fieldName;
        EnumEnumFriendlyName = enumFriendlyName;
        ValueAsText = valueAsText;
        Value = value;
        TripleSlashComments = tripleSlashComments;
        WasExplicitlySetAName = wasExplicitlySetAName;
        WasExplicitlySetAValue = wasExplicitlySetAValue;
    }

    // private MemberProperties(List<Diagnostic> errors)
    // {
    //     throw new System.NotImplementedException();
    // }

    public MemberSource Source { get; }
    
    public string FieldName { get; }

    public string EnumEnumFriendlyName { get; }
    
    public string ValueAsText { get; }

    public object Value { get; }
    
    public string TripleSlashComments { get; }
    
    /// <summary>
    /// An explicitly declared member has a name and value.
    /// An implicitly declared member has just a name.
    /// In the case of strings, new expressions don't even need a name
    /// as the name is implied from the field name.
    /// Also, for strings and ints, a field doesn't even need to be newed up
    /// as the name and value are inferred.
    /// </summary>
    public bool WasExplicitlySetAName { get; }
    
    public bool WasExplicitlySetAValue { get; }

    public bool NeedsInitializing => Source is MemberSource.FromFieldDeclator || !WasExplicitlySetAName || !WasExplicitlySetAValue;

    // public static MemberProperties? WithErrors(List<Diagnostic> errors)
    // {
    //     return new(errors);
    // }
}
