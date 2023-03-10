using System.Collections.Immutable;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests.LocalConfig;

public class HappyTests
{
    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    [InlineData("partial record class")]
    [InlineData("partial record struct")]
    [InlineData("readonly partial record struct")]
    public void Type_override(string type)
    {
        var source = $@"using System;
using Intellenum;
namespace Whatever;

[Intellenum(typeof(float))]
[Instance(""Normal"", 0f]
[Instance(""Gold"", 1f]
public {type} CustomerType {{ }}";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().BeEmpty();
        }
    }

    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    [InlineData("partial record class")]
    [InlineData("partial record struct")]
    [InlineData("readonly partial record struct")]
    public void Exception_override(string type)
    {
        var source = $@"using System;
using Intellenum;
namespace Whatever;

[Intellenum(throws: typeof(MyValidationException))]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType
{{
    private static Validation Validate(int value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}}

public class MyValidationException : Exception
{{
    public MyValidationException(string message) : base(message) {{ }}
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(0);
        }
    }

    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    [InlineData("partial record class")]
    [InlineData("partial record struct")]
    [InlineData("readonly partial record struct")]
    public void Conversion_override(string type)
    {
        var source = $@"using System;
using Intellenum;
namespace Whatever;

[Intellenum(conversions: Conversions.None)]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType {{ }}";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().BeEmpty();
        }
    }

    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    [InlineData("partial record class")]
    [InlineData("partial record struct")]
    [InlineData("readonly partial record struct")]
    public void Conversion_and_exceptions_override(string type)
    {
        var source = $@"using System;
using Intellenum;
namespace Whatever;

[Intellenum(conversions: Conversions.DapperTypeHandler, throws: typeof(Whatever.MyValidationException))]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType
{{
    private static Validation Validate(int value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}}


public class MyValidationException : Exception
{{
    public MyValidationException(string message) : base(message) {{ }}
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(0);
        }
    }

    [Theory]
    [InlineData("partial class")]
    [InlineData("partial struct")]
    [InlineData("readonly partial struct")]
    [InlineData("partial record class")]
    [InlineData("partial record struct")]
    [InlineData("readonly partial record struct")]
    public void Override_global_config_locally(string type)
    {
        var source = $@"using System;
using Intellenum;

[assembly: IntellenumDefaults(underlyingType: typeof(string), conversions: Conversions.None, throws:typeof(Whatever.MyValidationException))]

namespace Whatever;

[Intellenum(underlyingType:typeof(float))]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType
{{
    private static Validation Validate(float value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}}

public class MyValidationException : Exception
{{
    public MyValidationException(string message) : base(message) {{ }}
}}
";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().BeEmpty();
        }
    }
}
