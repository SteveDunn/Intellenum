using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Config;

[UsesVerify] 
public class GlobalConfigTests
{
    [Fact]
    public Task Type_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(underlyingType: typeof(float))]


namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial struct CustomerType
{
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task OmitDebugAttributes_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(debuggerAttributes: DebuggerAttributeGeneration.Basic)]


namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial struct CustomerType
{
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Exception_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(throws: typeof(Whatever.MyValidationException))]

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial struct CustomerType
{
    private static Validation Validate(int value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}

public class MyValidationException : Exception
{
    public MyValidationException(string message) : base(message) { }
}
";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Customization_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(customizations: Customizations.TreatNumberAsStringInSystemTextJson)]

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial struct CustomerType { }
";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Conversion_and_exceptions_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(conversions: Conversions.DapperTypeHandler, throws: typeof(Whatever.MyValidationException))]


namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial struct CustomerType
{
    private static Validation Validate(int value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}


public class MyValidationException : Exception
{
    public MyValidationException(string message) : base(message) { }
}
";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }

    [Fact]
    public Task Override_all()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(underlyingType: typeof(string), conversions: Conversions.None, throws:typeof(Whatever.MyValidationException))]

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public partial struct CustomerType
{
    private static Validation Validate(string value) => value.Length > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}

public class MyValidationException : Exception
{
    public MyValidationException(string message) : base(message) { }
}
";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .RunOnAllFrameworks();
    }
}
