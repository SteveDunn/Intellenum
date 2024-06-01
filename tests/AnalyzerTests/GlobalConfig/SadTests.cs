using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests.GlobalConfig;

public class SadTests
{
    [Fact]
    public async Task Not_valid_conversion()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(conversions: (Conversions)666)]

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial struct CustomerType { }
";

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
                        "(4,12): error INTELLENUM011: The Conversions specified do not match any known conversions - see the Conversions type");
                });
        }
    }

    [Fact]
    public async Task Not_valid_customization()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(customizations: (Customizations)666)]

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial struct CustomerType { }
";

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
                    first.Id.Should().Be("INTELLENUM019");
                    first.ToString().Should().Be(
                        "(4,12): error INTELLENUM019: The Customizations specified do not match any known customizations - see the Customizations type");
                });
        }
    }

    [Fact]
    public async Task Not_valid_customization_or_conversion()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(customizations: (Customizations)666, conversions: (Conversions)666)]

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial struct CustomerType { }
";

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(2);

            diagnostics.Should().SatisfyRespectively(
                first =>
                {
                    first.Id.Should().Be("INTELLENUM011");
                    first.ToString().Should().Be(
                        "(4,12): error INTELLENUM011: The Conversions specified do not match any known conversions - see the Conversions type");
                },
                second =>
                {
                    second.Id.Should().Be("INTELLENUM019");
                    second.ToString().Should().Be(
                        "(4,12): error INTELLENUM019: The Customizations specified do not match any known customizations - see the Customizations type");
                });
        }
    }
}
