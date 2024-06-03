using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis;

// a wrapper around diagnostics for easier testing
namespace AnalyzerTests;

public class DiagnosticCollection
{
    private readonly ImmutableArray<Diagnostic> _items;

    public DiagnosticCollection(ImmutableArray<Diagnostic> items) => _items = items;

    public int Count => _items.Length;

    public void HasId(string id) => _items.Any(i => i.Id == id).Should().BeTrue();

    public void HasError(string error) => _items.Any(i => i.GetMessage() == error).Should().BeTrue();
    
    public void ShouldHaveError(string id, string error) => _items.Should().Contain(i => i.Id == id && i.GetMessage(null) == error);
    

    public void ShouldHaveErrorStartingWith(string error) => _items.Any(i => i.GetMessage().StartsWith(error));
    public void ShouldHaveErrorStartingWith(string id, string error) => _items.Any(i => i.Id == id && i.GetMessage().StartsWith(error)).Should().BeTrue();

    public void ShouldHaveErrorContaining(string error) => _items.Any(i => i.GetMessage().Contains(error)).Should().BeTrue();
    public void ShouldHaveErrorContaining(string id, string error) => _items.Any(i => i.Id == id && i.GetMessage().Contains(error)).Should().BeTrue();
}