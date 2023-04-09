using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace SnapshotTests;

public static class Factory
{
    public static readonly ImmutableHashSet<string> UnderlyingTypes = new List<string>
    {
        "", // don't include underlying type - should default to int
        "int",
        "string",
        "decimal",
#if THOROUGH
            "byte",
            "char",
            "bool",
            "System.DateTimeOffset",
            "System.DateTime",
            "double",
            "float",
            "System.Guid",
            "long",
            "short",
#endif
    }.ToImmutableHashSet();

    public static string MemberAttributeFor(string type)
    {
        return type switch
        {
            "" => @"[Member(""One"", 1)]", // don't include underlying type - should default to int
            "int" => @"[Member(""One"", 1)]",
            "string" => @"[Member(""One"", ""1"")]",
            "decimal" => throw new Exception("decimal not supported for attributes"),
#if THOROUGH
            "byte" => @"[Member(""One"", 1)]",
            "char" => @"[Member(""One"", '1')]",
            "bool" => @"[Member(""One"", true)]",
            "System.DateTimeOffset" => @"[Member(""One"", DateTimeOffset.MinValue)]",
            "System.DateTime" => @"[Member(""One"", DateTime.MinValue)]",
            "double" => @"[Member(""One"", 1d)]",
            "float" => @"[Member(""One"", 1f)]",
            "System.Guid" => @"[Member(""One"", System.Guid.Empty)]",
            "long" => @"[Member(""One"", 1l)]",
            "short" => @"[Member(""One"", 1)]",
#endif
            _ => throw new Exception("Nothing for '{type}'")
        };
    }

    public static string MemberCallFor(string type)
    {
        return type switch
        {
            "" => @"Member(""One"", 1);", // don't include underlying type - should default to int
            "int" => @"Member(""One"", 1);",
            "string" => @"Member(""One"", ""1"");",
            "decimal" => @"Member(""One"", 1m);",
            "byte" => @"Member(""One"", 1);",
            "char" => @"Member(""One"", '1');",
            "bool" => @"Member(""One"", true);",
            "System.DateTimeOffset" => @"Member(""One"", System.DateTimeOffset.MinValue);",
            "System.DateTime" => @"Member(""One"", System.DateTime.MinValue);",
            "double" => @"Member(""One"", 1d);",
            "float" => @"Member(""One"", 1f);",
            "System.Guid" => @"Member(""One"", System.Guid.Empty);",
            "long" => @"Member(""One"", 1l);",
            "short" => @"Member(""One"", 1);",
            _ => throw new Exception("Nothing for '{type}'")
        };
    }
}