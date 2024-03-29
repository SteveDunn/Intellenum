﻿using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Intellenum;
using Microsoft.CodeAnalysis;

namespace AnalyzerTests;

public class DisallowNonPartialTests
{
    [Theory]
    [InlineData("abstract class")]
    [InlineData("abstract record class")]
    public void Disallows_non_partial_types(string type)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Instance(""Normal"", 0)]
[Instance(""Gold"", 1)]
public {type} CustomerType {{ }}
";
        new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(Validate)
            .RunOnAllFrameworks();

        void Validate(ImmutableArray<Diagnostic> diagnostics)
        {
            diagnostics.Should().HaveCount(1);
            Diagnostic diagnostic = diagnostics.Single();

            diagnostic.Id.Should().Be("VOG021");
            diagnostic.ToString().Should()
                .Match("*: error VOG021: Type CustomerId is decorated as a Value Object and should be in a partial type.");
        }
    }
}