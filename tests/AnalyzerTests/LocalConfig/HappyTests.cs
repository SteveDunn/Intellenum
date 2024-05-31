using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests.LocalConfig;

public class HappyTests
{
    [Fact]
    public async Task Type_override()
    {
        var source = $$"""
                       using System;
                       using Intellenum;
                       namespace Whatever;

                       [Intellenum(typeof(float))]
                       [Member("Normal", 0f)]
                       [Member("Gold", 1f)]
                       public partial class CustomerType { }
                       """;

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task Conversion_override()
    {
        var source = $$"""
                       using System;
                       using Intellenum;
                       namespace Whatever;

                       [Intellenum(conversions: Conversions.None)]
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
            diagnostics.Should().BeEmpty();
        }
    }

    [Fact]
    public async Task Override_global_config_locally()
    {
        var source = $$"""
                       using System;
                       using Intellenum;

                       [assembly: IntellenumDefaults(underlyingType: typeof(string), conversions: Conversions.None)]

                       namespace Whatever;

                       [Intellenum(underlyingType:typeof(float))]
                       [Member("Normal", 0)]
                       [Member("Gold", 1)]
                       public partial class CustomerType
                       {
                       }
                       """;

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().BeEmpty();
        }
    }
}
