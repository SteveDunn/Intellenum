using System.Collections.Immutable;
using System.Linq;
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
        public void parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType(int SomethingElse)
{{
}}
";

            new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {
                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("VOG008");
                diagnostic.ToString().Should().Match(
                    "*VOG008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Theory]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public void multiple_parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType(int SomethingElse, string Name, int Age)
{{
}}
";

            new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {
                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("VOG008");
                diagnostic.ToString().Should().Match(
                    "*VOG008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Theory]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public void empty_parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType()
{{
}}
";

            new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {
                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("VOG008");
                diagnostic.ToString().Should().Match(
                    "*VOG008: Cannot have user defined constructors, please use the From method for creation.");
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
        public void parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType
{{
    public CustomerType() {{ }}
}}
";

            new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {

                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("VOG008");
                diagnostic.ToString().Should().Match(
                    "*VOG008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Theory]
        [InlineData("partial class")]
        [InlineData("partial struct")]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public void multiple_parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType
{{
    public CustomerType(int SomethingElse, string Name, int Age) {{ }}
}}
";

            new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {

                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("VOG008");
                diagnostic.ToString().Should().Match(
                    "*VOG008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }

        [Theory]
        [InlineData("partial class")]
        [InlineData("partial struct")]
        [InlineData("partial record class")]
        [InlineData("partial record struct")]
        [InlineData("readonly partial record struct")]
        public void empty_parameters_disallowed(string type)
        {
            var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType
{{
    public CustomerType() {{ }}
}}
";
            new TestRunner<IntellenumGenerator>()
                .WithSource(source)
                .ValidateWith(Validate)
                .RunOnAllFrameworks();

            void Validate(ImmutableArray<Diagnostic> diagnostics)
            {

                diagnostics.Should().HaveCount(1);
                Diagnostic diagnostic = diagnostics.Single();

                using var scope = new AssertionScope();
                diagnostic.Id.Should().Be("VOG008");
                diagnostic.ToString().Should().Match(
                    "*VOG008: Cannot have user defined constructors, please use the From method for creation.");
            }
        }
    }
}