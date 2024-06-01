using System;
using System.Collections.Immutable;
using System.IO;
using Intellenum;
using Microsoft.CodeAnalysis.Testing;

namespace AnalyzerTests
{
    public static class References
    {
        static readonly string _loc = typeof(IntellenumAttribute).Assembly.Location;

        public static Lazy<ReferenceAssemblies> Net80AndOurs = new(() =>
            new ReferenceAssemblies(
                    "net8.0",
                    new PackageIdentity(
                        "Microsoft.NETCore.App.Ref",
                        "8.0.0"),
                    Path.Combine("ref", "net8.0"))
                .AddAssemblies(
                    ["Intellenum", "Intellenum.SharedTypes", _loc.Replace(".dll", string.Empty)]));
    }
}