using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class DisallowDuplicateAttributesTests
{
    [Theory]
    [InlineData("abstract partial class")]
    [InlineData("abstract partial record class")]
    public void Disallows_multiple_value_object_attributes(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Intellenum]
public {type} CustomerId {{ }}
";
        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(1);
            Diagnostic diagnostic = diagnostics.Single();

            diagnostic.Id.Should().Be("CS0579");
            diagnostic.ToString().Should()
                .Match("* error CS0579: Duplicate 'ValueObject' attribute");
        }
    }
}