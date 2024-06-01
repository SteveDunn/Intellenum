using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Intellenum;

namespace AnalyzerTests;

public class ToStringOverrideTests
{
    [Theory]
    [ClassData(typeof(Types))]
    public async Task Do_not_override_ToString_if_it_is_supplied(string type, string sealedOrNot)
    {
        var source = $@"using Intellenum;

namespace Whatever;

[Intellenum]
[Member(""Normal"", 0)]
[Member(""Gold"", 1)]
{type} CustomerType
{{
    public override {sealedOrNot} string ToString() => string.Empty;
}}
";

        await new TestRunner<IntellenumGenerator>()
            .WithSource(source)
            .ValidateWith(d => d.Should().HaveCount(0))
            .RunOnAllFrameworks();
    }
    
    private class Types : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            foreach (string type in _types)
            {
                yield return
                [
                    type,
                    "sealed"
                ];

                yield return
                [
                    type,
                    string.Empty
                ];
            }
        }

        private readonly string[] _types =
        [
            "public partial class",
            "public sealed partial class",
            "internal partial class",
            "internal sealed partial class"
        ];

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
    
}