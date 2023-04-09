using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Config;

[UsesVerify]
public class LocalConfigTests
{
    [Fact]
    public Task OmitDebugAttributes_override()
    {
        var source = @"using System;
using Intellenum;
namespace Whatever;

[Intellenum(debuggerAttributes: DebuggerAttributeGeneration.Basic)]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Defaults()
    {
        var source = @"using System;
using Intellenum;
namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Defaults_with_validation_and_instances()
    {
        var source = @"using System;
using Intellenum;
namespace Whatever;

[Intellenum]
[Member(name: ""Basic"", value: 0, tripleSlashComment: ""a short description that'll show up in intellisense"")]
[Member(name: ""Gold"", value: 1, tripleSlashComment: ""another short description that'll show up in intellisense"")]
public partial class CustomerType
{
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Type_override()
    {
        var source = @"using System;
using Intellenum;
namespace Whatever;

[Intellenum(typeof(float))]
[Member(""Normal"", 0.1f)]
[Member(""Gold"", 0.2f)]
public partial class CustomerType
{
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Conversion_override()
    {
        var source = @"using System;
using Intellenum;
namespace Whatever;

[Intellenum(conversions: Conversions.None)]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType { }";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Override_global_config_locally()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(underlyingType: typeof(string), conversions: Conversions.None)]

namespace Whatever;

[Intellenum(underlyingType:typeof(float))]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
public partial class CustomerType
{
}
";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }
}
