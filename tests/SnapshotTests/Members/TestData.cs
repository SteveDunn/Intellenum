using System.Collections;
using System.Collections.Generic;

namespace SnapshotTests.Members;

public class TestData : IEnumerable<object[]>
{
    public IEnumerator<object[]> GetEnumerator()
    {
        string type = "partial class";
        
        foreach ((string underlyingType, string memberValue) in _underlyingTypes)
        {
            var qualifiedType = "public " + type;
            yield return new object[]
                { qualifiedType, underlyingType, memberValue, CreateClassName(qualifiedType, underlyingType) };

            qualifiedType = "internal " + type;
            yield return new object[]
                { qualifiedType, underlyingType, memberValue, CreateClassName(qualifiedType, underlyingType) };
        }
    }

    private static string CreateClassName(string type, string underlyingType) =>
        type.Replace(" ", "_") + underlyingType;

    // for each of the attributes above, use this underlying type
    private readonly (string underlyingType, string memberValue)[] _underlyingTypes = new[]
    {
        ("byte", "42"),
        ("char", "'x'"),
        ("double", "123.45d"),
        ("float", "123.45f"),
        ("int", "123"),
        ("long", "123L"),
        ("string", """123"""),
    };

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}