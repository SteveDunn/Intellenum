using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;
using VerifyXunit;

namespace AnalyzerTests
{
    [UsesVerify]
    public class MemberTests
    {
        public class When_there_are_members
        {
            [Fact]
            public Task Of_a_customType()
            {
                string source = $$"""
using System;
using Intellenum;

namespace Whatever;

[Intellenum<Foo>]
public partial class FooEnum
{
    // just for the test - it's generated in real life
    // public FooEnum(string name, Foo value) { }


    public static readonly FooEnum Item1 = new("Item1", new Foo("a", 1));
    public static readonly FooEnum Item2 = new("Item2", new Foo("b", 2));
}

public record class Foo(string Name, int Age) : IComparable<Foo>
{
    public int CompareTo(Foo other) => Age.CompareTo(other.Age);
}

""";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(validate)
                    .RunOnAllFrameworks(true);

                void validate(ImmutableArray<Diagnostic> diagnostics)
                {
                    DiagnosticCollection d = new(diagnostics);

                    d.Count.Should().Be(0);
                }

                return Task.CompletedTask;
            }

        }

        public class When_there_are_no_members_a_diagnostic_is_emitted
            {
            
            [Fact]
            public Task No_members()
            {
                string declaration = $@"using System;
  [Intellenum]
  public partial class MyMembersTests {{ }}";
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
                    DiagnosticCollection d = new(diagnostics);

                    d.Count.Should().Be(1);
                    
                    d.HasError("INTELLENUM026", "MyMembersTests must have at least 1 member");
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
  [Member(name: ""Invalid"", value: ""1.23x"")]
  public partial class MyMemberTests {{ }}";
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
                    DiagnosticCollection d = new(diagnostics);
                    d.Count.Should().Be(2);
#if NET7_0_OR_GREATER
                    d.HasError("INTELLENUM023", "MyMemberTests cannot be converted. Member value named Invalid has an attribute with a 'System.String' of '1.23x' which cannot be converted to the underlying type of 'System.Single' - The input string '1.23x' was not in a correct format.");
#else
                    d.HasError("INTELLENUM023", "MyMemberTests cannot be converted. Member value named Invalid has an attribute with a 'System.String' of '1.23x' which cannot be converted to the underlying type of 'System.Single' - Input string was not in a correct format.");

#endif
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
    [Member(name: ""Invalid"", value: ""x2022-13-99"")]
    public partial class MyMemberTests { }
}";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(validate)
                    .RunOnAllFrameworks();

                void validate(ImmutableArray<Diagnostic> diagnostics)
                {
                    diagnostics.Should().HaveCount(2);
                    diagnostics.Should().ContainSingle(d => d.GetMessage(null).Contains(
                        "The string 'x2022-13-99' was not recognized as a valid DateTime") && d.Id == "INTELLENUM023");
                    diagnostics.Should().ContainSingle(d => d.Id == "INTELLENUM026");
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
    [Member(name: ""Invalid"", value: ""x2022-13-99"")]
    public partial class MyMemberTests { }
}";

                new TestRunner<IntellenumGenerator>()
                    .WithSource(source)
                    .ValidateWith(Validate)
                    .RunOnAllFrameworks();

                void Validate(ImmutableArray<Diagnostic> diagnostics)
                {
                    var d = new DiagnosticCollection(diagnostics);
                    d.Count.Should().Be(2);
                    d.HasErrorContaining("MyMemberTests cannot be converted. Member value named Invalid has an attribute with a 'System.String' of 'x2022-13-99' which cannot be converted to the underlying type of 'System.DateTimeOffset'");
                    d.HasId("INTELLENUM023");
                }

                return Task.CompletedTask;
            }
        }
    }
}