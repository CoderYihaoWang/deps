using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Yihao.Deps.Tests;

public class GraphGeneratorTests
{
    [Fact]
    public void TestGenerate()
    {
        var generator = new GraphGenerator();  
        var input = JsonSerializer.Deserialize<List<Repo>>(File.ReadAllText(Path.Combine("assets", "Input.json")));
        var output = $@"```mermaid
{generator.Generate(input!)}```
";
        var expected = File.ReadAllText(Path.Combine("assets", "Output.md"));

        output.Should().Be(expected);
    }
}