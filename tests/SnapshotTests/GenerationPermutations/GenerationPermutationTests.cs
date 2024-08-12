using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;
using Xunit.Abstractions;

namespace SnapshotTests.GenerationPermutations;

[UsesVerify] 
public class GenerationPermutationTests
{
    private readonly ITestOutputHelper _logger;

    public GenerationPermutationTests(ITestOutputHelper logger) => _logger = logger;

    public class Types : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            string conversions =
                "Conversions.NewtonsoftJson | Conversions.SystemTextJson | Conversions.EfCoreValueConverter | Conversions.DapperTypeHandler | Conversions.LinqToDbValueConverter";
            foreach (string underlyingType in Factory.UnderlyingTypes)
            {
                foreach (string accessModifier in _accessModifiers)
                {
                    var qualifiedType = $"{accessModifier} partial class";
                    yield return
                    [
                        qualifiedType,
                        Factory.MemberCallFor(underlyingType),
                        conversions,
                        underlyingType,
                        CreateClassName(qualifiedType, conversions, underlyingType)
                    ];
                }
            }
        }

        private static string CreateClassName(string type, string conversion, string underlyingType) =>
            Normalize($"{type}{conversion}{underlyingType}");

        private static string Normalize(string input) => input.Replace(" ", "_").Replace("|", "_").Replace(".", "_").Replace("@", "_");


        private readonly string[] _accessModifiers =
        {
            "public"
#if THOROUGH
            , "internal"
#endif
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private Task Run(string type, string memberCall, string conversions, string underlyingType, string className, string locale)
    {
        _logger.WriteLine($"Running permutation, type: {type}, conversion: {conversions}, underlyingType: {underlyingType}, className: {className}, locale: {locale}");

        string declaration;

        if (underlyingType.Length == 0)
        {
            declaration = $$"""
    [Intellenum(conversions: {{conversions}})]
    {{type}} {{className}} 
    {
        static {{className}}() {
            {{memberCall}}
        }
    }
""";
        }
        else
        {
            declaration = $$"""
    [Intellenum(conversions: {{conversions}}, underlyingType: typeof({{underlyingType}}))]
    {{type}} {{className}} { 

        static {{className}}() {
            {{memberCall}}
        }
    }
""";
        }

        var source = @"using Intellenum;
namespace Whatever
{
" + declaration + @"
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .IgnoreInitialCompilationErrors()
            .WithLogger(_logger)
            .WithSource(source)
            .WithLocale(locale)
            .CustomizeSettings(s => s.UseFileName(TestHelper.ShortenForFilename(className)))
            .RunOnAllFrameworks();
    }

    [Theory]
    [ClassData(typeof(Types))]
    public Task GenerationTest(string type, string memberCall, string conversions, string underlyingType, string className) => Run(
        type,
        memberCall,
        conversions,
        underlyingType,
        className,
        string.Empty);

#if THOROUGH
    [Theory]
    [UseCulture("fr-FR")]
    [ClassData(typeof(Types))]
    public Task GenerationTests_FR(string type, string attribute, string conversions, string underlyingType, string className) => Run(
        type,
        attribute,
        conversions,
        underlyingType,
        className,
        "fr");

    [Theory]
    [UseCulture("en-US")]
    [ClassData(typeof(Types))]
    public Task GenerationTests_US(string type, string attribute, string conversions, string underlyingType, string className) => Run(
        type,
        attribute,
        conversions,
        underlyingType,
        className,
        "us");
#endif
}