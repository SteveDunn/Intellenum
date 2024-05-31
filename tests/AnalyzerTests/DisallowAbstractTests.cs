using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class DisallowAbstractTests
{
    [Theory]
    [InlineData("abstract partial class")]
    [InlineData("abstract partial record class")]
    public async Task Disallows_abstract_value_objects(string type)
    {
        var source = $$"""
                       using Intellenum;

                       namespace Whatever;

                       [Intellenum]
                       [Member("Normal", 0)]
                       [Member("Gold", 1)]
                       public {{type}} CustomerType { }
                       """;
        
        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();
        
        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(1);
            Diagnostic diagnostic = diagnostics.Single();

            diagnostic.Id.Should().Be("INTELLENUM017");
            diagnostic.ToString()
                .Should()
                .Match("* error INTELLENUM017: Type 'CustomerType' cannot be abstract");
        }
    }


    [Fact]
    public async Task Disallows_nested_abstract_value_objects()
    {
        var source = $$"""
                       using Intellenum;

                       namespace Whatever;

                       public class MyContainer {
                           [Intellenum]
                           [Member("Normal", 0)]
                           [Member("Gold", 1)]
                           public abstract partial class CustomerType { }
                       }

                       """;

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(2);
            Diagnostic diagnostic = diagnostics.ElementAt(0);

            diagnostic.Id.Should().Be("INTELLENUM017");
            diagnostic.ToString().Should()
                .Match("*error INTELLENUM017: Type 'CustomerType' cannot be abstract");

            diagnostic = diagnostics.ElementAt(1);

            diagnostic.Id.Should().Be("INTELLENUM001");
            diagnostic.ToString().Should()
                .Match("*error INTELLENUM001: Type 'CustomerType' cannot be nested - remove it from inside MyContainer");
        }
    }
}