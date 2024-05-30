using System.Threading.Tasks;
using Intellenum;
using Shared;
using VerifyXunit;

namespace SnapshotTests;

[UsesVerify]
public class GenericAttributeTests
{
    [SkippableFact]
    public Task Partial_class_created_successfully()
    {
        var source = @"using Intellenum;
namespace Whatever;

[Intellenum<int>]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{
}";

        return RunTest(source);
    }

    private static Task RunTest(string source) =>
        new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net8_0);

    [SkippableFact]
    public Task No_namespace() =>
        RunTest(@"using Intellenum;

[Intellenum<int>]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{
}");


    [SkippableFact]
    public Task Produces_members()
    {
        return RunTest(@"using Intellenum;

namespace Whatever;

[Intellenum<int>]
[Member(name: ""Unspecified"", value: -1, tripleSlashComment: ""a short description that'll show up in intellisense"")]
[Member(name: ""Unspecified1"", value: -2)]
[Member(name: ""Unspecified2"", value: -3, tripleSlashComment: ""<some_xml>whatever</some_xml"")]
[Member(name: ""Unspecified3"", value: -4)]
[Member(name: ""Preferred"", value: 42)]
public partial class CustomerType
{
}
");
    }

    [SkippableFact]
    public Task Produces_members_with_derived_attribute()
    {
        return RunTest(@"using Intellenum;

namespace Whatever;

public class CustomGenericAttribute : IntellenumAttribute<long>
{
    public CustomGenericAttribute(Conversions conversions = Conversions.Default | Conversions.EfCoreValueConverter)
    {
    }
}

[CustomGenericAttribute]
[Member(name: ""Unspecified"", value: -1, tripleSlashComment: ""a short description that'll show up in intellisense"")]
[Member(name: ""Unspecified1"", value: -2)]
[Member(name: ""Unspecified2"", value: -3, tripleSlashComment: ""<some_xml>whatever</some_xml"")]
[Member(name: ""Unspecified3"", value: -4)]
[Member(name: ""Cust42"", value: 42)]
public partial class CustomerType
{
}
");
    }
    
    [SkippableFact]
    public Task Member_names_can_have_reserved_keywords()
    {
        return RunTest("""
            using Intellenum;

            namespace Whatever;

            [Intellenum<int>]
            [Member(name: "@class", value: 42)]
            [Member(name: "@event", value: 69)]
            public partial class CustomerType
            {
            }

            """);
    }

    [SkippableFact]
    public Task Namespace_names_can_have_reserved_keywords()
    {
        return RunTest(@"using Intellenum;

namespace @double;

[Intellenum<int>]
[Member(name: ""@struct"", value: 42)]
[Member(name: ""@event"", value: 69)]
[Member(name: ""@void"", value: 666)]
public partial class @class
{
}
");
    }
}
