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
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial class CustomerType
{
}";

        return RunTest(source);
    }

    private static Task RunTest(string source) =>
        new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .RunOn(TargetFramework.Net7_0);

    [SkippableFact]
    public Task No_namespace() =>
        RunTest(@"using Intellenum;

[Intellenum<int>]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial class CustomerType
{
}");


    [SkippableFact]
    public Task Produces_instances()
    {
        return RunTest(@"using Intellenum;

namespace Whatever;

[Intellenum<int>]
[Instance(name: ""Unspecified"", value: -1, tripleSlashComment: ""a short description that'll show up in intellisense"")]
[Instance(name: ""Unspecified1"", value: -2)]
[Instance(name: ""Unspecified2"", value: -3, tripleSlashComment: ""<some_xml>whatever</some_xml"")]
[Instance(name: ""Unspecified3"", value: -4)]
[Instance(name: ""Preferred"", value: 42)]
public partial class CustomerType
{
}
");
    }

    [SkippableFact]
    public Task Produces_instances_with_derived_attribute()
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
[Instance(name: ""Unspecified"", value: -1, tripleSlashComment: ""a short description that'll show up in intellisense"")]
[Instance(name: ""Unspecified1"", value: -2)]
[Instance(name: ""Unspecified2"", value: -3, tripleSlashComment: ""<some_xml>whatever</some_xml"")]
[Instance(name: ""Unspecified3"", value: -4)]
[Instance(name: ""Cust42"", value: 42)]
public partial class CustomerType
{
}
");
    }
    
    [SkippableFact]
    public Task Instance_names_can_have_reserved_keywords()
    {
        return RunTest("""
            using Intellenum;

            namespace Whatever;

            [Intellenum<int>]
            [Instance(name: "@class", value: 42)]
            [Instance(name: "@event", value: 69)]
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
[Instance(name: ""@struct"", value: 42)]
[Instance(name: ""@event"", value: 69)]
[Instance(name: ""@void"", value: 666)]
public partial class @class
{
}
");
    }
}
