using System.Text.Json.Nodes;

namespace Benchmarks;

public static class Combiner
{
    public static void CombineResults(
        string resultsDir = "./BenchmarkDotNet.Artifacts/results",
        string resultsFile = "Combined.Benchmarks",
        string searchPattern = "*-report-full-compressed.json")
    {
        var resultsPath = Path.Combine(resultsDir, resultsFile + ".json");
        
        Console.Out.WriteLine($"Combining benchmark results in results folder at {resultsDir}");

        if (!Directory.Exists(resultsDir))
        {
            throw new DirectoryNotFoundException($"Directory not found '{resultsDir}'");
        }
        
        Console.Out.WriteLine($"Combining results into file {resultsPath}");
        
        if (File.Exists(resultsPath))
        {
            Console.Out.WriteLine("Results file already exists. Deleting.");

            File.Delete(resultsPath);
        }

        var reports = Directory
            .GetFiles(resultsDir, searchPattern, SearchOption.TopDirectoryOnly)
            .ToArray();
        
        if (!reports.Any())
        {
            throw new FileNotFoundException($"Reports not found '{searchPattern}'");
        }

        var combinedReport = JsonNode.Parse(File.ReadAllText(reports.First()))!;
        
        var title = combinedReport["Title"]!;
        
        var benchmarks = combinedReport["Benchmarks"]!.AsArray();
        
        // Rename title whilst keeping original timestamp
        combinedReport["Title"] = $"{resultsFile}{title.GetValue<string>()[^16..]}";

        foreach (var report in reports.Skip(1))
        {
            var array = JsonNode.Parse(File.ReadAllText(report))!["Benchmarks"]!.AsArray();
            foreach (var benchmark in array)
            {
                // Double parse avoids "The node already has a parent" exception
                benchmarks.Add(JsonNode.Parse(benchmark!.ToJsonString())!);
            }
        }

        File.WriteAllText(resultsPath, combinedReport.ToString());
    }
}