using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Members;

[UsesVerify] 
public class MemberGenerationTests
{
    [Fact]
    public Task Members_can_be_booleans()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum(typeof(bool))]
[Member("Invalid", false)]
public partial class BooleanThing
{
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Member_names_can_have_reserved_keywords()
    {
        var source = @"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(name: ""@class"", value: 42)]
[Member(name: ""@event"", value: 69)]
public partial class CSharpSymbol
{
}
";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Theory]
    [UseCulture("fr-FR")]
    [ClassData(typeof(TestData))]
    public Task GenerationTest_FR(string type, string underlyingType, string memberValue,
        string className) => Run(
        type,
        underlyingType,
        memberValue,
        className,
        "fr");

    [Theory]
    [ClassData(typeof(TestData))]
    public Task GenerationTest(string type, string underlyingType, string memberValue,
        string className) => Run(
        type,
        underlyingType,
        memberValue,
        className,
        "");

    private Task Run(string type, string underlyingType, string memberValue, string className, string locale)
    {
        var source = $$"""
                       using Intellenum;
                       namespace Whatever
                       {
                         [Intellenum(underlyingType: typeof({{underlyingType}}))]
                         [Member(name: "MyValue", value: {{memberValue}})]
                         {{type}} {{className}} {}
                       }
                       """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .WithLocale(locale)
            .CustomizeSettings(s => s.UseFileName(TestHelper.ShortenForFilename(className)))
            .RunOnAllFrameworks();
    }
}