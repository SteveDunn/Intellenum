using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests.LocalConfig;

public class SadTests
{
    [Fact]
    public async Task Not_valid_conversion()
    {
        var source = $$"""
                       using System;
                       using Intellenum;

                       namespace Whatever;

                       [Intellenum(conversions: (Conversions)666)]
                       [Member("Normal", 0)]
                       [Member("Gold", 1)]
                       public partial class CustomerType { }

                       """;

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(1);

            diagnostics.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be("INTELLENUM011");
                    first.ToString().Should().Be(
                        "(6,2): error INTELLENUM011: The Conversions specified do not match any known conversions - see the Conversions type");
                });
        }
    }
}
