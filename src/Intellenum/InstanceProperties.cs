namespace Intellenum;

public class InstanceProperties
{
    public InstanceProperties(InstanceSource source,
        string name,
        string valueAsText,
        object value,
        string tripleSlashComments = "")
    {
        Source = source;
        Name = name;
        ValueAsText = valueAsText;
        Value = value;
        TripleSlashComments = tripleSlashComments;
    }

    public InstanceSource Source { get; }
    
    public string Name { get; }
    
    public string ValueAsText { get; }

    public object Value { get; }
    
    public string TripleSlashComments { get; }
}
