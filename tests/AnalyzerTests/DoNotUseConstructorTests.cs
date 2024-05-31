using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class DoNotUseConstructorTests
{
    public class PrimaryConstructorTests
    {
        [Theory]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public async Task parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public {type} CustomerType(int SomethingElse)
{{
}}
";

            await new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {
                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("INTELLENUM008");
                diagnostic.ToString().Should().Match(
                    "*INTELLENUM008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Theory]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public async Task multiple_parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public {type} CustomerType(int SomethingElse, string Name, int Age)
{{
}}
";

            await new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {
                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("INTELLENUM008");
                diagnostic.ToString().Should().Match(
                    "*INTELLENUM008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Theory]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public async Task empty_parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public {type} CustomerType()
{{
}}
";

            await new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {
                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("INTELLENUM008");
                diagnostic.ToString().Should().Match(
                    "*INTELLENUM008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }
    }

    public class ConstructorTests
    {
        [Theory]
        [InlineData("partial class")]
        [InlineData("partial struct")]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public async Task parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public {type} CustomerType
{{
    public CustomerType() {{ }}
}}
";

            await new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {

                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("INTELLENUM008");
                diagnostic.ToString().Should().Match(
                    "*INTELLENUM008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Fact]
        public async Task multiple_parameters_disallowed()
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{{
    public CustomerType(int SomethingElse, string Name, int Age) {{ }}
}}
";

            await new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {

                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("INTELLENUM008");
                diagnostic.ToString().Should().Match(
                    "*INTELLENUM008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Fact]
        public async Task empty_parameters_disallowed()
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{{
    public CustomerType() {{ }}
}}
";
            await new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {

                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("INTELLENUM008");
                diagnostic.ToString().Should().Match(
                    "*INTELLENUM008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }
    }
}