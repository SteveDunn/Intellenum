using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.Escaping;

[UsesVerify]
public class GenerationOfEscapedTypesTests
{
    public class Types : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            string type = "partial class";
            
            foreach (string conversion in _conversions)
            {
                foreach (string underlyingType in _underlyingTypes)
                {
                    var qualifiedType = "public " + type;
                    yield return new object[]
                    {
                        qualifiedType, conversion, underlyingType,
                        CreateClassName(qualifiedType, conversion, underlyingType)
                    };

                    qualifiedType = "internal " + type;
                    yield return new object[]
                    {
                        qualifiedType, conversion, underlyingType,
                        CreateClassName(qualifiedType, conversion, underlyingType)
                    };
                }
            }
        }

        private static string CreateClassName(string type, string conversion, string underlyingType) =>
            Normalize($"escapedTests{type}{conversion}{underlyingType}");

        private static string Normalize(string input) => 
            input.Replace(" ", "_").Replace("|", "_").Replace(".", "_").Replace("@", "_");
        
        // for each of the types above, create classes for each one of these attributes
        private readonly string[] _conversions = new[]
        {
            "Conversions.None",
            "Conversions.TypeConverter",
            "Conversions.NewtonsoftJson",
            "Conversions.SystemTextJson",
            "Conversions.NewtonsoftJson | Conversions.SystemTextJson",
            "Conversions.EfCoreValueConverter",
            "Conversions.DapperTypeHandler",
            "Conversions.LinqToDbValueConverter",
        };

        // for each of the attributes above, use this underlying type
        private readonly string[] _underlyingTypes = 
        {
            "byte",
            "double",
            "System.Guid",
            "string",
            "record.@struct.@float.@decimal",
            "record.@struct.@float.@event2",
            "record.@struct.@float.@event",
        };

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(Types))]
    public Task GenerationOfEscapedTypes(string type, string conversions, string underlyingType, string className)
    {
        string instanceCall = Factory.MemberCallFor(underlyingType);
        string declaration = $$"""
namespace record.@struct.@float
{
    public readonly record struct @decimal(decimal V) : System.IComparable<@decimal>
    {
        public int CompareTo(@decimal other) => 1;
    }

    public readonly record struct @event2(decimal V) : System.IComparable<@event2>
    {
        public int CompareTo(@event2 other) => 1;
    }

    public readonly record struct @event(decimal V) : System.IComparable<@event>
    {
        public int CompareTo(@event other) => 1;
    }
}

  [Intellenum(conversions: {{conversions}}, underlyingType: typeof({{underlyingType}}))]
  {{type}} {{className}} {
        static {{className}}() {
            {{instanceCall}}
        }
    }
""";
        
        var source = @"using Intellenum;
namespace @class
{
" + declaration + @"
}";

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .IgnoreInitialCompilationErrors()
            .IgnoreFinalCompilationErrors()
            .CustomizeSettings(s => s.UseFileName(className))
            .RunOnAllFrameworks();
    }
}