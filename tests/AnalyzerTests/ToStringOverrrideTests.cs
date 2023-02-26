using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class ToStringOverrideTests
{
    [Theory]
    [InlineData("public partial record class")]
    [InlineData("public partial record")]
    public void WithRecordsThatHaveNoSealedOverride_OutputErrors(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
{type} CustomerId 
{{
    public override string ToString() => string.Empty;
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

            diagnostic.Id.Should().Be("VOG020");
            diagnostic.ToString().Should().Match(
                "*: error VOG020: ToString overrides should be sealed on records. See https://github.com/SteveDunn/Vogen/wiki/Records#tostring for more information.");
        }
    }

    [Theory]
    [InlineData("public partial record class")]
    [InlineData("public partial record")]
    public void WithRecordsThatDoHaveSealedOverride_DoNotOutputErrors(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
{type} CustomerId 
{{
    public override sealed string ToString() => string.Empty;
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(d => d.Should().HaveCount(0))
            .RunOnAllFrameworks();
    }
    
    [Fact]
    public void RecordStruct_DoesNotRequireSealedToString()
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
public partial record struct CustomerId 
{{
    public override string ToString() => string.Empty;
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(d => d.Should().HaveCount(0))
            .RunOnAllFrameworks();
    }

    [Theory]
    [ClassData(typeof(Types))]
    public void WithNonRecordsThatHaveMixtureOfSealedAndNonSealedOverrides_DoNotOutputErrors(string type, string sealedOrNot)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
{type} CustomerId 
{{
    public override {sealedOrNot} string ToString() => string.Empty;
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(d => d.Should().HaveCount(0))
            .RunOnAllFrameworks();
    }
    
    private class Types : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (string type in _types)
            {
                yield return new object[]
                {
                    type,
                    "sealed"
                };

                yield return new object[]
                {
                    type,
                    string.Empty
                };
            }
        }

        private readonly string[] _types =
        {
            "public partial class",
            "internal partial class"
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
}