using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = AnalyzerTests.Verifiers.CSharpAnalyzerVerifier<Intellenum.Rules.DoNotUseNewAnalyzer>;
// ReSharper disable CoVariantArrayConversion

namespace AnalyzerTests
{
    public class DoNotUseNewAnalyzerTests
    {
        //No diagnostics expected to show up
        [Fact]
        public async Task NoDiagnosticsForEmptyCode()
        {
            var test = @"";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task Allow_new_for_newing_up_inside_of_type_itself()
        {
            var source = @"using Intellenum;
namespace Whatever;

[Intellenum(typeof(int))]
public partial class Condiments 
{
    // just for the test - it's generated in real life
    public Condiments(string name, int value) { }
    
    public static readonly Condiments Salt = new Condiments(""Salt"", 1); 
}
";
            await Run(
                source,
                Enumerable.Empty<DiagnosticResult>());
        }

        [Fact]
        public async Task Disallow_new_for_newing_up_outside_of_type_itself()
        {
            var source = $@"using Intellenum;
namespace Whatever;

[Intellenum(typeof(int))]
public partial class MyVo {{ }}

public class Test {{
    public Test() {{
        var c = {{|#0:new MyVo()|}};
        MyVo c2 = {{|#1:new()|}};
    }}
}}
";
            await Run(
                source,
                WithDiagnostics("VOG010", DiagnosticSeverity.Error, "MyVo", 0, 1));
        }

        [Fact]
        public async Task Disallow_new_for_method_return_type()
        {
            var source = $@"
using Intellenum;
namespace Whatever;

[Intellenum]
public partial class MyVo {{ }}

public class Test {{
    public MyVo Get() => {{|#0:new MyVo()|}};
    public MyVo Get2() => {{|#1:new MyVo()|}};
}}
";

            await Run(
                source,
                WithDiagnostics("VOG010", DiagnosticSeverity.Error, "MyVo", 0, 1));
        }

        [Fact]
        public async Task Disallow_new_from_local_function()
        {
            var source = $@"
using Intellenum;
namespace Whatever;

[Intellenum]
public partial class MyVo {{ }}

public class Test {{
    public Test() {{
        MyVo Get() => {{|#0:new MyVo()|}};
        MyVo Get2() => {{|#1:new()|}};
    }}
}}
";

            await Run(
                source,
                WithDiagnostics("VOG010", DiagnosticSeverity.Error, "MyVo", 0, 1));
        }

        [Fact]
        public async Task Disallow_new_from_func()
        {
            var source = $@"
using System;
using System.Threading.Tasks;
using Intellenum;
namespace Whatever;

[Intellenum]
public partial class MyVo {{ }}

public class Test {{
        Func<MyVo> f = () =>  {{|#0:new MyVo()|}};
        Func<MyVo> f2 = () =>  {{|#1:new()|}};
        Func<int, int, MyVo, string, MyVo> f3 = (a,b,c,d) =>  {{|#2:new MyVo()|}};
        Func<int, int, MyVo, string, MyVo> f4 = (a,b,c,d) =>  {{|#3:new()|}};
        Func<int, int, MyVo, string, Task<MyVo>> f5 = async (a,b,c,d) => await Task.FromResult({{|#4:new MyVo()|}});
}}
";

            await Run(
                source,
                WithDiagnostics("VOG010", DiagnosticSeverity.Error, "MyVo", 0, 1, 2, 3, 4));
        }

        [Fact(DisplayName = "Bug https://github.com/SteveDunn/Vogen/issues/182")]
        public async Task Analyzer_false_position_for_implicit_new_in_array_initializer()
        {
            var source = @"using System;
using System.Threading.Tasks;
using Intellenum;

public class Test {
    Vo c = Create(new Object[]
    {
        // This call is the issue
        new()
    });

    static Vo Create(Object[] normalObject)
    {
        throw null; // we don't actually generate the VO in this test
    }
}

[Intellenum(typeof(int))]
public partial class Vo { }";

            await Run(
                source,
                Enumerable.Empty<DiagnosticResult>());
        }

        private static IEnumerable<DiagnosticResult> WithDiagnostics(string code, DiagnosticSeverity severity,
            string arguments, params int[] locations)
        {
            foreach (var location in locations)
            {
                yield return VerifyCS.Diagnostic(code).WithSeverity(severity).WithLocation(location)
                    .WithArguments(arguments);
            }
        }

        private async Task Run(string source, IEnumerable<DiagnosticResult> expected)
        {
            var test = new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { source },
                },

                CompilerDiagnostics = CompilerDiagnostics.Errors,
                ReferenceAssemblies = References.Net70AndOurs.Value,
            };

            test.ExpectedDiagnostics.AddRange(expected);

            await test.RunAsync();
        }
    }
}
