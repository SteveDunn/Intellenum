using System;

namespace Intellenum;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class MemberAttribute : Attribute
{
    public object Value { get; }

    public string Name { get; }
    
    public string TripleSlashComment { get; }

    public MemberAttribute(string name) => Name = name;
    
    public MemberAttribute(string name, object value) => (Name, Value) = (name, value);

    public MemberAttribute(string name, object value, string tripleSlashComment) =>
        (Name, Value, TripleSlashComment) = (name, value, tripleSlashComment);
}