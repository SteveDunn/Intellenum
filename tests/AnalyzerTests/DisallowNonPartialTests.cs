using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class DisallowNonPartialTests
{
    [Theory]
    [InlineData("abstract class")]
    [InlineData("abstract record class")]
    public async Task Disallows_non_partial_types(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public {type} CustomerType {{ }}
";
        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(1);
            Diagnostic diagnostic = diagnostics.Single();

            diagnostic.Id.Should().Be("INTELLENUM021");
            diagnostic.ToString().Should()
                .Match("*: error INTELLENUM021: Type CustomerType is decorated as an Intellenum and should be in a partial type.");
        }
    }
}