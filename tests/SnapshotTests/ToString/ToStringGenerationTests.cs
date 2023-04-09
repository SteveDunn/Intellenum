using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Intellenum;
using VerifyXunit;

namespace SnapshotTests.ToString;

[UsesVerify]
public class ToStringGenerationTests
{
    private class Types : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                CreateClassName(ToStringMethod.None),
                ToStringMethod.None,
            };

            yield return new object[]
            {
                CreateClassName(ToStringMethod.Method),
                ToStringMethod.Method,
            };

            yield return new object[]
            {
                CreateClassName(ToStringMethod.ExpressionBodiedMethod),
                ToStringMethod.ExpressionBodiedMethod,
            };
        }

        private static string CreateClassName(ToStringMethod toStringMethod) => $"partial_class_{toStringMethod}";

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    [Theory]
    [ClassData(typeof(Types))]
    public Task Test(string className, ToStringMethod addToStringMethod)
    {
        var source = $$"""
            using Intellenum;
            namespace Whatever
            {
              [Intellenum]
              [Member("One", 1)]
              public partial class {{className}} { 
                {{WriteToStringMethod(addToStringMethod)}} 
              }
            }
            """;

        return new SnapshotRunner<IntellenumGenerator>()
            .WithSource(source)
            .CustomizeSettings(s => s.UseFileName(className))
            .RunOnAllFrameworks();
    }

    private static string WriteToStringMethod(ToStringMethod toStringMethod)
    {
        string s = "public override string ToString()";
        return toStringMethod switch
        {
            ToStringMethod.None => string.Empty,
            ToStringMethod.Method => $"{s} {{return \"!\"; }}",
            ToStringMethod.ExpressionBodiedMethod => $"{s} => \"!\";",
            _ => throw new InvalidOperationException($"Don't know what a {toStringMethod} is!")
        };
    }
}

    public enum ToStringMethod
    {
        None,
        Method,
        ExpressionBodiedMethod
    }