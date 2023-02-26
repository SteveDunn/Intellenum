using System.Collections.Immutable;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests.GlobalConfig;

public class HappyTests
{
    [Fact]
    public void Type_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(underlyingType: typeof(float))]


namespace Whatever;

[Intellenum]
public partial struct CustomerId
{
}";

        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().BeEmpty();
        }
    }

    [Fact]
    public void Exception_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(throws: typeof(Whatever.MyValidationException))]

namespace Whatever;

[Intellenum]
public partial struct CustomerId
{
    private static Validation Validate(int value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}

public class MyValidationException : Exception
{
    public MyValidationException(string message) : base(message) { }
}
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

    [Fact]
    public void Conversion_and_exceptions_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(conversions: Conversions.DapperTypeHandler, throws: typeof(Whatever.MyValidationException))]


namespace Whatever;

[Intellenum]
public partial struct CustomerId
{
    private static Validation Validate(int value) => value > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}


public class MyValidationException : Exception
{
    public MyValidationException(string message) : base(message) { }
}
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

    [Fact]
    public void DeserializationStrictness_override()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(conversions: Conversions.DapperTypeHandler, deserializationStrictness: DeserializationStrictness.AllowAnything)]


namespace Whatever;

[Intellenum]
public partial struct CustomerId { }
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

    [Fact]
    public void Override_all()
    {
        var source = @"using System;
using Intellenum;

[assembly: IntellenumDefaults(underlyingType: typeof(string), conversions: Conversions.None, throws:typeof(Whatever.MyValidationException), deserializationStrictness: DeserializationStrictness.AllowAnything)]

namespace Whatever;

[Intellenum]
public partial struct CustomerId
{
    private static Validation Validate(string value) => value.Length > 0 ? Validation.Ok : Validation.Invalid(""xxxx"");
}

public class MyValidationException : Exception
{
    public MyValidationException(string message) : base(message) { }
}
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
