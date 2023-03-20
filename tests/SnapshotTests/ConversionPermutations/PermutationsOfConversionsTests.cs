using System.Linq;
using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.ConversionPermutations;

[UsesVerify]
public class PermutationsOfConversionsTests
{

    static readonly string[] _permutations = new Permutations().ToArray();

    // These used to be 'ClassData' tests, but they were run sequentially, which was very slow.
    // This test now runs the permutations in parallel.
    [Fact]
    public async Task CompilesWithAnyCombinationOfConverters()
    {
        string type = "partial class";
        foreach (var conversions in _permutations)
        {
            await RunTest(
                $@"
  [Intellenum(conversions: {conversions}, underlyingType: typeof(int))]
  [Instance(""One"", 1)]
  public {type} MyIntVo {{ }}",
                type,
                conversions);
        }
    }


    private static Task RunTest(string declaration, string type, string conversions)
    {
        var source = $@"using System;
using Intellenum;
namespace Whatever
{{
{declaration}
}}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .CustomizeSettings(
                s =>
                {
                    var typeHash = type.Replace(' ', '-');
                    string parameters = typeHash + TestHelper.ShortenForFilename(conversions);
                    s.UseFileName(parameters);

                })
            .RunOnAllFrameworks();
    }
}