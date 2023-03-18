using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;
using VerifyXunit;

namespace AnalyzerTests
{
    [UsesVerify]
    public class InstanceFieldTests
    {
        public class When_there_are_no_instances_a_diagnostic_is_emitted
        {
            [Fact]
            public Task No_instances()
            {
                string declaration = $@"using System;
  [Intellenum]
  public partial class MyInstanceTests {{ }}";
                var source = @"using Intellenum;
namespace Whatever
{
" + declaration + @"
}";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(validate)
                    .RunOnAllFrameworks();

                void validate(ImmutableArray<Diagnostic> diagnostics)
                {
                    diagnostics.Single().GetMessage().Should().Be(
                        "MyInstanceTests must have at least 1 instance");

                    diagnostics.Single().Id.Should().Be("VOG026");
                }

                return Task.CompletedTask;
            }
        }
        
        
        
        public class When_values_cannot_be_converted_to_their_underlying_types
        {
            [Fact]
            public Task Malformed_float_causes_compilation_error()
            {
                string declaration = $@"using System;
  [Intellenum(underlyingType: typeof(float))]
  [Instance(name: ""Invalid"", value: ""1.23x"")]
  public partial class MyInstanceTests {{ }}";
                var source = @"using Intellenum;
namespace Whatever
{
" + declaration + @"
}";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(Validate)
                    .RunOnAllFrameworks();

                void Validate(ImmutableArray<Diagnostic> diagnostics)
                {
#if NET7_0_OR_GREATER
            diagnostics.Single().GetMessage().Should().Be(
                "MyInstanceTests cannot be converted. Instance value named Invalid has an attribute with a 'System.String' of '1.23x' which cannot be converted to the underlying type of 'System.Single' - The input string '1.23x' was not in a correct format.");
#else
                    diagnostics.Single().GetMessage().Should().Be(
                        "MyInstanceTests cannot be converted. Instance value named Invalid has an attribute with a 'System.String' of '1.23x' which cannot be converted to the underlying type of 'System.Single' - Input string was not in a correct format.");

#endif
                    diagnostics.Single().Id.Should().Be("VOG023");
                }

                return Task.CompletedTask;
            }

            [Fact]
            public Task Malformed_datetime_causes_compilation_error()
            {
                var source = @"
using Intellenum;
using System;
namespace Whatever
{
    [Intellenum(underlyingType: typeof(DateTime))]
    [Instance(name: ""Invalid"", value: ""x2022-13-99"")]
    public partial class MyInstanceTests { }
}";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(Validate)
                    .RunOnAllFrameworks();

                void Validate(ImmutableArray<Diagnostic> diagnostics)
                {
                    diagnostics.Should().HaveCount(1);
                    diagnostics.Single().GetMessage().Should().Contain(
                        "MyInstanceTests cannot be converted. Instance value named Invalid has an attribute with a 'System.String' of 'x2022-13-99' which cannot be converted to the underlying type of 'System.DateTime'");

                    diagnostics.Single().Id.Should().Be("VOG023");
                }

                return Task.CompletedTask;
            }

            [Fact]
            public Task Malformed_DateTimeOffset_causes_compilation_error()
            {
                var source = @"
using Intellenum;
using System;
namespace Whatever
{
    [Intellenum(underlyingType: typeof(DateTimeOffset))]
    [Instance(name: ""Invalid"", value: ""x2022-13-99"")]
    public partial class MyInstanceTests { }
}";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(Validate)
                    .RunOnAllFrameworks();

                void Validate(ImmutableArray<Diagnostic> diagnostics)
                {
                    diagnostics.Should().HaveCount(1);
                    diagnostics.Single().GetMessage().Should().Contain(
                        "MyInstanceTests cannot be converted. Instance value named Invalid has an attribute with a 'System.String' of 'x2022-13-99' which cannot be converted to the underlying type of 'System.DateTimeOffset'");

                    diagnostics.Single().Id.Should().Be("VOG023");
                }

                return Task.CompletedTask;
            }
        }
    }
}