using System.Threading.Tasks;
using Intellenum;
using Shared;
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
    public Task Mixture_with_strings()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<string>]
[Members("Zero, One, Two, Three")]
[Member("Four")]
[Member("Five")]
[Member("Six")]
public partial class Mixture
{
    static Mixture()
    {
        Member("Nine");
        Member("Ten");
        Members("Eleven, Twelve");
        Member("Thirteen");
    }
  
    public static readonly Mixture Seven = new();
    public static readonly Mixture Eight = new("Eight");
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net8_0);
    }
    
    // It doesn't really make sense to use this style of creation, especially for ints.
    // Values are assigned in the order specified in the source generator, not the order that they're specified
    // in the code. For instance, in this example, the `Member` attributes are processed first, then the `Members` attribute,
    // then the static constructor `Member` methods, then the field declarations.
    //
    // It makes _more_ sense to use this style with strings, as the values are the same as the name.
    [Fact]
    public Task Mixture_with_ints()
    {
        var source = """
using Intellenum;

namespace Whatever;

[Intellenum<int>]
[Members("Zero, One, Two, Three")]
[Member("Four")]
[Member("Five")]
[Member("Six")]
public partial class Mixture
{
    static Mixture()
    {
        Member("Nine");
        Member("Ten");
        Members("Eleven, Twelve");
        Member("Thirteen");
    }
  
    public static readonly Mixture Seven = new();
    public static readonly Mixture Eight = new(888);
}
""";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net8_0);
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