using System.Text.Json;
using Yihao.Deps;

var dirs = args.Distinct();
var missing = dirs.Where(d => !Directory.Exists(d));

if (missing.Any())
{
    Console.WriteLine("Error: some repositories does not exist, please make sure you are passing in the correct directories:");
    foreach(var d in missing)
    {
        Console.WriteLine($"  {d}");
    }
}
    
var analyzer = new RepoAnalyzer();
var repos = dirs.Select(analyzer.Analyze);
File.WriteAllText("deps.json", JsonSerializer.Serialize(repos, new JsonSerializerOptions
{
    WriteIndented = true,
}));

var generator = new GraphGenerator();
var graph = generator.Generate(repos);
File.WriteAllText("deps.md", graph);
