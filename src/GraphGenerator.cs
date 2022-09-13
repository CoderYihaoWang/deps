using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yihao.Deps;

public class GraphGenerator
{
    public string Generate(IEnumerable<Repo> repos)
    {
        var sb = new StringBuilder();
        using var writer = new StringWriter(sb);

        writer.WriteLine("graph");

        var ids = new Dictionary<string, string>();
        var curId = 1;

        foreach (var repo in repos)
        {
            foreach (var project in repo.Projects.OrderBy(p => p.Name))
            {
                var id = $"_{curId++}";
                ids.Add(project.Name, id);
            }
        }

        foreach (var repo in repos)
        {
            writer.WriteLine($"subgraph {repo.Name}");
            WriteNodes(repo, ids, writer);
            WriteProjectRefs(repo, ids, writer);
            writer.WriteLine("end");
        }

        foreach (var repo in repos)
        {
            WritePackageRefs(repo, ids, writer);
        }

        return sb.ToString();
    }

    void WriteNodes(Repo repo, IDictionary<string, string> ids, StringWriter writer)
    {
        foreach (var project in repo.Projects.OrderBy(p => p.Name))
        {
            writer.WriteLine($"  {ids[project.Name]}[\"{project.Name}\"]");
        }
    }

    void WriteProjectRefs(Repo repo, IDictionary<string, string> ids, StringWriter writer)
    {
        foreach (var project in repo.Projects.OrderBy(p => p.Name))
        {
            foreach (var pr in project.ProjectRefs.OrderBy(p => p))
            {
                writer.WriteLine($"  {ids[project.Name]} --> {ids[pr]}");
            }
        }
    }

    void WritePackageRefs(Repo repo, IDictionary<string, string> ids, StringWriter writer)
    {
        foreach (var project in repo.Projects.OrderBy(p => p.Name))
        {
            foreach (var pr in project.PackageRefs.OrderBy(p => p))
            {
                if (ids.ContainsKey(pr))
                {
                    writer.WriteLine($"{ids[project.Name]} --> {ids[pr]}");
                }
            }
        }
    }
}

