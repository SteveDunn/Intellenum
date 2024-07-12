using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = AnalyzerTests.Verifiers.CSharpAnalyzerVerifier<Intellenum.Rules.DuplicateMembersAnalyzer>;
// ReSharper disable CoVariantArrayConversion

namespace AnalyzerTests;

public class DuplicateMembersAnalyzerTests
{
    //No diagnostics expected to show up
    [Fact]
    public async Task NoDiagnosticsForEmptyCode()
    {
        var test = @"";
        await VerifyCS.VerifyAnalyzerAsync(test);
    }
        
    [Fact]
    public async Task No_diagnostic_for_non_repeat()
    {
        var source = $$"""
                       using Intellenum;

                       namespace Whatever;

                       [Intellenum<int>]
                       [Member("One")]
                       public partial class MyEnum 
                       { 
                       }
                       """;
        await Run(source,[]);
    }

    [Fact]
    public async Task No_diagnostic_for_non_repeat2()
    {
        var source = $$"""
                       using Intellenum;
                       
                       namespace Whatever;
                       
                       [Intellenum<string>]
                       [Member("Good", "Good")]
                       [Member("Average", "Average")]
                       [Member("Bad", "Bad")]
                       public partial class VendorRating { }
                       """;
        await Run(source,[]);
    }

    [Fact]
    public async Task No_diagnostic_for_non_repeat3()
    {
        var source = $$"""
                       using Intellenum;
                       
                       namespace Whatever;
                       
                       [Intellenum]
                       [Member("Won", 2)]
                       [Member("Drawn", 1)]
                       [Member("Lost", 0)]
                       internal partial class Result
                       {
                       }
                       """;
        await Run(source,[]);
    }

    [Fact]
    public async Task No_diagnostic_for_non_repeat4()
    {
        var source = $$"""
                       using Intellenum;
                       
                       namespace Whatever;
                       
                       [Intellenum]
                       internal partial class Result
                       {
                          public static Result One = new();
                          public static Result Two = new();
                          public static Result Three = new(3);
                       }
                       """;
        var test = new VerifyCS.Test
        {
            TestState = { Sources = { source } },
            CompilerDiagnostics = CompilerDiagnostics.None,
            ReferenceAssemblies = References.Net80AndOurs.Value,
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task No_diagnostic_for_non_repeat5()
    {
        var source = $$"""
                       using Intellenum;
                       
                       namespace Whatever;
                       
                       
                       [Intellenum<int>]
                       [Member("Standard", 0)]
                       [Member("Gold", 1)]
                       [Member("Diamond", 2)]
                       public partial class CustomerType;
                       """;
        var test = new VerifyCS.Test
        {
            TestState = { Sources = { source } },
            CompilerDiagnostics = CompilerDiagnostics.None,
            ReferenceAssemblies = References.Net80AndOurs.Value,
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task No_diagnostic_for_non_repeat_with_complex_object()
    {
        var source = $$"""
                       using Intellenum;
                       
                       namespace Whatever;
                       
                       public class Person
                       {
                            public string Name { get; }
                            public int Age { get; }
                            
                            public Person(string name, int age)
                            {
                                Name = name;
                                Age = age;
                                Address = address;
                            }
                       }
                       
                       [Intellenum<Person>]
                       internal partial class Result
                       {
                          public static Result One = new(new("Fred", 42));
                       }
                       """;
        var test = new VerifyCS.Test
        {
            TestState = { Sources = { source } },
            CompilerDiagnostics = CompilerDiagnostics.None,
            ReferenceAssemblies = References.Net80AndOurs.Value,
        };

        await test.RunAsync();
    }

    [Fact]
    public async Task Spots_duplicates_in_fields()
    {
        var source = $$"""
                       using System;
                       using Intellenum;

                       namespace Whatever;

                       [Intellenum<int>]
                       public partial class {|#0:MyEnum|}
                       {
                           public static MyEnum One = new();
                           public static MyEnum Two = new();
                           public static MyEnum Three = new(3);
                           public static MyEnum Four = new(3);
                       }
                       """;

        IEnumerable<DiagnosticResult> expected1 = [
            VerifyCS.Diagnostic("INTELLENUM030").WithArguments("MyEnum", "The members named: Three, Four - repeat the value '3'").WithSpan(7, 22, 7, 28)
        ];
        
        var test = new VerifyCS.Test
        {
            TestState = { Sources = { source } },
            CompilerDiagnostics = CompilerDiagnostics.None,
            ReferenceAssemblies = References.Net80AndOurs.Value,
        };

        test.ExpectedDiagnostics.AddRange(expected1);

        await test.RunAsync();
    }

    private static async Task Run(string source, IEnumerable<DiagnosticResult> expected)
    {
        var test = new VerifyCS.Test
        {
            TestState =
            {
                Sources = { source },
            },

            CompilerDiagnostics = CompilerDiagnostics.None,
            
            ReferenceAssemblies = References.Net80AndOurs.Value,
        };

        test.ExpectedDiagnostics.AddRange(expected);

        await test.RunAsync();
    }
}