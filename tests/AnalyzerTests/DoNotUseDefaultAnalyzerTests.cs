using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = AnalyzerTests.Verifiers.CSharpAnalyzerVerifier<Intellenum.Rules.DoNotUseDefaultAnalyzer>;
// ReSharper disable CoVariantArrayConversion

namespace AnalyzerTests;

public class DoNotUseDefaultAnalyzerTests
{
    //No diagnostics expected to show up
    [Fact]
    public async Task NoDiagnosticsForEmptyCode()
    {
        var test = @"";
        await VerifyCS.VerifyAnalyzerAsync(test);
    }

    [Fact]
    public async Task Disallow_default_when_creating_variable()
    {
        var source = $$"""
                       using System;
                       using Intellenum;
                       
                       namespace Whatever;

                       [Intellenum(typeof(int))]
                       public partial class MyVo { }

                       public class Test {
                           public Test() {
                               MyVo c = {|#0:default|};
                               MyVo c2 = {|#1:default(MyVo)|};
                           }
                       }

                       """;
        await Run(source, VerifyCS.Diagnostics("INTELLENUM009", DiagnosticSeverity.Error, "MyVo", [0, 1]));
    }

    [Fact]
    public async Task Disallow_default_for_method_return_type()
    {
        var source = $$"""
                       using System;
                       using Intellenum;

                       namespace Whatever;

                       [Intellenum]
                       public partial class MyVo { }

                       public class Test {
                           public MyVo Get() => {|#0:default|};
                           public MyVo Get2() => {|#1:default(MyVo)|};
                       }
                       """;

        int[] locations = [0, 1];
        await Run(source, VerifyCS.Diagnostics("INTELLENUM009", DiagnosticSeverity.Error, "MyVo", locations));
    }

    [Fact]
    public async Task Disallow_default_from_local_function_return()
    {
        var source = $$"""
                       using System;
                       using Intellenum;
                       namespace Whatever;

                       [Intellenum]
                       public partial class MyVo { }

                       public class Test {
                           public Test() {
                               MyVo Get() => {|#0:default|};
                               MyVo Get2() => {|#1:default(MyVo)|};
                           }
                       }
                       """;

        int[] locations = [0, 1];
        await Run(
            source,
            VerifyCS.Diagnostics("INTELLENUM009", DiagnosticSeverity.Error, "MyVo", locations));
    }

    [Fact]
    public async Task Disallow_default_from_func()
    {
        var source = $$"""
                       using System;
                       using System.Threading.Tasks;
                       using Intellenum;
                       namespace Whatever;

                       [Intellenum]
                       public partial class MyVo { }

                       public class Test {
                               Func<MyVo> f = () =>  {|#0:default(MyVo)|};
                               Func<MyVo> f2 = () =>  {|#1:default|};
                               Func<int, int, MyVo, string, MyVo> f3 = (a,b,c,d) =>  {|#2:default(MyVo)|};
                               Func<int, int, MyVo, string, MyVo> f4 = (a,b,c,d) =>  {|#3:default|};
                               Func<int, int, MyVo, string, Task<MyVo>> f5 = async (a,b,c,d) => await Task.FromResult({|#4:default(MyVo)|});
                       }
                       """;

        await Run(source, VerifyCS.Diagnostics("INTELLENUM009", DiagnosticSeverity.Error, "MyVo", [0, 1, 2, 3, 4]));
    }

    private async Task Run(string source, IEnumerable<DiagnosticResult> expected)
    {
        VerifyCS.Test test = new VerifyCS.Test
        {
            TestState =
            {
                Sources = { source },
            },

            CompilerDiagnostics = CompilerDiagnostics.Errors,
            ReferenceAssemblies = References.Net80AndOurs.Value,
        };

        test.ExpectedDiagnostics.AddRange(expected);

        await test.RunAsync();
    }
}