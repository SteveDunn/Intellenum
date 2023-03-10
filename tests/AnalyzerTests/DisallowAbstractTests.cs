using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class DisallowAbstractTests
{
    [Theory]
    [InlineData("abstract partial class")]
    [InlineData("abstract partial record class")]
    public void Disallows_abstract_value_objects(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType {{ }}
";
        
        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();
        
        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(1);
            Diagnostic diagnostic = diagnostics.Single();

            diagnostic.Id.Should().Be("VOG017");
            diagnostic.ToString()
                .Should()
                .Match("* error VOG017: Type 'CustomerType' cannot be abstract");
        }
    }


    [Theory]
    [InlineData("abstract partial class")]
    [InlineData("abstract partial record class")]
    public void Disallows_nested_abstract_value_objects(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

public class MyContainer {{
    [Instance(""Normal"", 0)]
    [Instance(""Gold"", 1)]
    public {type} CustomerType {{ }}
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(2);
            Diagnostic diagnostic = diagnostics.ElementAt(0);

            diagnostic.Id.Should().Be("VOG017");
            diagnostic.ToString().Should()
                .Match("*error VOG017: Type 'CustomerType' cannot be abstract");

            diagnostic = diagnostics.ElementAt(1);

            diagnostic.Id.Should().Be("VOG001");
            diagnostic.ToString().Should()
                .Match("*error VOG001: Type 'CustomerType' cannot be nested - remove it from inside MyContainer");
        }
    }
}