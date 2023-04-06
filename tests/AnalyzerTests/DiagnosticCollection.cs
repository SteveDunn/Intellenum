using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;

// a wrapper around diagnostics for easier testing
namespace AnalyzerTests;

public class DiagnosticCollection
{
    private readonly ImmutableArray<Diagnostic> _items;

    public DiagnosticCollection(ImmutableArray<Diagnostic> items) => _items = items;

    public int Count => _items.Length;

    public bool HasId(string id) => _items.Any(i => i.Id == id);

    public bool HasError(string error) => _items.Any(i => i.GetMessage() == error);
    
    public bool HasError(string id, string error) => _items.Any(i => i.Id == id && i.GetMessage() == error);
    

    public bool HasErrorStartingWith(string error) => _items.Any(i => i.GetMessage().StartsWith(error));
    public bool HasErrorStartingWith(string id, string error) => _items.Any(i => i.Id == id && i.GetMessage().StartsWith(error));

    public bool HasErrorContaining(string error) => _items.Any(i => i.GetMessage().Contains(error));
    public bool HasErrorContaining(string id, string error) => _items.Any(i => i.Id == id && i.GetMessage().Contains(error));
}