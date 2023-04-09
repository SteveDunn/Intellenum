using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using VerifyCS = AnalyzerTests.Verifiers.CSharpAnalyzerVerifier<Intellenum.Rules.DoNotUseReflectionAnalyzer>;

namespace AnalyzerTests
{
    public class DoNotUseReflectionAnalyzerTests
    {
        //No diagnostics expected to show up
        [Fact]
        public async Task NoDiagnosticsForEmptyCode()
        {
            var test = @"";
            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        [Fact]
        public async Task Disallows_generic_method()
        {
            var source = $@"using Intellenum;
using System;

namespace Whatever;

public class Test {{
    public Test() {{
        var c = {{|#0:Activator.CreateInstance<MyVo>()|}};
    }}
}}

[Intellenum(typeof(int))]
public partial class MyVo {{ }}
";
            
            await Run(
                source,
                WithDiagnostics("INTELLENUM025", DiagnosticSeverity.Error, 0));
        }

        [Fact]
        public async Task Disallows_non_generic_method()
        {
            var source = $@"using Intellenum;
using System;

namespace Whatever;

public class Test {{
    public Test() {{
        var c = {{|#0:Activator.CreateInstance(typeof(MyVo))|}};
    }}
}}

[Intellenum(typeof(int))]
public partial class MyVo {{ }}
";
            
            await Run(
                source,
                WithDiagnostics("INTELLENUM025", DiagnosticSeverity.Error, 0));
        }

        private static IEnumerable<DiagnosticResult> WithDiagnostics(string code, DiagnosticSeverity severity, params int[] locations)
        {
            foreach (var location in locations)
            {
                yield return VerifyCS.Diagnostic(code).WithSeverity(severity).WithLocation(location)
                    .WithArguments("MyVo");
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
