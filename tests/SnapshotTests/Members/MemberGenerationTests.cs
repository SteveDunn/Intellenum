using System.Collections;
using System.Collections.Generic;
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
        string declaration = $@"
  [Intellenum(underlyingType: typeof({underlyingType}))]
  [Member(name: ""MyValue"", value: {memberValue})]
  {type} {className} {{}}";
        var source = @"using Intellenum;
namespace Whatever
{
" + declaration + @"
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .WithLocale(locale)
            .CustomizeSettings(s => s.UseFileName(TestHelper.ShortenForFilename(className)))
            .RunOnAllFrameworks();
    }
}

public class TestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        string type = "partial class";
        
        foreach ((string underlyingType, string memberValue) in _underlyingTypes)
        {
            var qualifiedType = "public " + type;
            yield return new object[]
                { qualifiedType, underlyingType, memberValue, CreateClassName(qualifiedType, underlyingType) };

            qualifiedType = "internal " + type;
            yield return new object[]
                { qualifiedType, underlyingType, memberValue, CreateClassName(qualifiedType, underlyingType) };
        }
    }

    private static string CreateClassName(string type, string underlyingType) =>
        type.Replace(" ", "_") + underlyingType;

    // for each of the attributes above, use this underlying type
    private readonly (string underlyingType, string memberValue)[] _underlyingTypes = new[]
    {
        ("byte", "42"),
        ("char", "'x'"),
        ("double", "123.45d"),
        ("float", "123.45f"),
        ("int", "123"),
        ("long", "123L"),
        ("string", """123"""),
    };

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
