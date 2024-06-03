using System.Collections.Immutable;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;
using VerifyXunit;

namespace AnalyzerTests;

[UsesVerify]
public class MembersAttributeTests
{
    [Fact]
    public async Task Cannot_be_applied_to_enums_that_are_not_based_on_int()
    {
        string source = $$"""
                          using Intellenum;
                          namespace Whatever;

                          [Intellenum<float>]
                          [Members("Normal, Gold, Diamond")]
                          public partial class CustomerType
                          {
                          }
                          """;

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks(true);

        static void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            DiagnosticCollection d = new(diagnostics);

            d.Count.Should().Be(1);

            d.ShouldHaveError("INTELLENUM027", "The type 'CustomerType' cannot have a Members attribute because it is not based on int");
        }
    }
}