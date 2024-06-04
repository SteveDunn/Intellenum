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

    public void ShouldHaveCountOf(int count) => _items.Should().HaveCount(count);
    
    public void ShouldBeEmpty() => _items.Should().BeEmpty();

    public void ShouldHaveError(string id, string error) => _items.Should().Contain(i => i.Id == id && i.GetMessage(null) == error);
    

    public void ShouldHaveErrorStartingWith(string id, string error) => _items.Should().Contain(i => i.Id == id && i.GetMessage(null).StartsWith(error));

    public void ShouldHaveErrorContaining(string error) => _items.Any(i => i.GetMessage().Contains(error)).Should().BeTrue();

    public void ShouldHaveErrorContaining(string id, string error) =>
        _items.Should().Contain(i => i.Id == id && i.GetMessage(null).Contains(error));
}