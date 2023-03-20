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

    public static string InstanceAttributeFor(string type)
    {
        return type switch
        {
            "" => @"[Instance(""One"", 1)]", // don't include underlying type - should default to int
            "int" => @"[Instance(""One"", 1)]",
            "string" => @"[Instance(""One"", ""1"")]",
            "decimal" => throw new Exception("decimal not supported for attributes"),
#if THOROUGH
            "byte" => @"[Instance(""One"", 1)]",
            "char" => @"[Instance(""One"", '1')]",
            "bool" => @"[Instance(""One"", true)]",
            "System.DateTimeOffset" => @"[Instance(""One"", DateTimeOffset.MinValue)]",
            "System.DateTime" => @"[Instance(""One"", DateTime.MinValue)]",
            "double" => @"[Instance(""One"", 1d)]",
            "float" => @"[Instance(""One"", 1f)]",
            "System.Guid" => @"[Instance(""One"", System.Guid.Empty)]",
            "long" => @"[Instance(""One"", 1l)]",
            "short" => @"[Instance(""One"", 1)]",
#endif
            _ => throw new Exception("Nothing for '{type}'")
        };
    }

    public static string InstanceCallFor(string type)
    {
        return type switch
        {
            "" => @"Instance(""One"", 1);", // don't include underlying type - should default to int
            "int" => @"Instance(""One"", 1);",
            "string" => @"Instance(""One"", ""1"");",
            "decimal" => @"Instance(""One"", 1m);",
            "byte" => @"Instance(""One"", 1);",
            "char" => @"Instance(""One"", '1');",
            "bool" => @"Instance(""One"", true);",
            "System.DateTimeOffset" => @"Instance(""One"", System.DateTimeOffset.MinValue);",
            "System.DateTime" => @"Instance(""One"", System.DateTime.MinValue);",
            "double" => @"Instance(""One"", 1d);",
            "float" => @"Instance(""One"", 1f);",
            "System.Guid" => @"Instance(""One"", System.Guid.Empty);",
            "long" => @"Instance(""One"", 1l);",
            "short" => @"Instance(""One"", 1);",
            _ => throw new Exception("Nothing for '{type}'")
        };
    }
}