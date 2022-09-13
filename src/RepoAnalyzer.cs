using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Yihao.Deps;

public record Project(string Name, IEnumerable<string> PackageRefs, IEnumerable<string> ProjectRefs);
public record Repo(string Name, IEnumerable<Project> Projects);

public class RepoAnalyzer
{
    public Repo Analyze(string directory)
    {
        var files = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories)
            .Where(f => !f.Contains("_build"))
            .Select(Path.GetFullPath);

        var docs = files.ToDictionary(f => f, _ => new XmlDocument());
        foreach (var (f, d) in docs)
        {
            d.Load(f);
        }

        var assemblyNames = files.ToDictionary(f => f, f => GetAssemblyName(f, docs[f]));

        var projects = new List<Project>();
        foreach (var f in files)
        {
            var packageRefs = GetPackageRefs(docs[f]);
            var projectRefs = GetProjectRefs(f, docs[f], assemblyNames);
            projects.Add(new Project(assemblyNames[f], packageRefs, projectRefs));
        }

        return new Repo(directory.Split(Path.DirectorySeparatorChar)[^1], projects);
    }

    IEnumerable<string> GetPackageRefs(XmlDocument doc)
        => doc.GetElementsByTagName("PackageReference")
            .Cast<XmlElement>()
            .Select(e => e.GetAttribute("Include"));

    IEnumerable<string> GetProjectRefs(string filename, XmlDocument doc, IDictionary<string, string> assemblyNames)
        => doc.GetElementsByTagName("ProjectReference")
            .Cast<XmlElement>()
            .Select(e => e.GetAttribute("Include"))
            .Select(pr => NormalizeProjectRef(filename, pr, assemblyNames));

    string GetAssemblyName(string filename, XmlDocument doc)
        =>  doc.GetElementsByTagName("AssemblyName").Item(0)?.InnerText ?? filename;
   
    string NormalizeProjectRef(string projectPath, string projectRef, IDictionary<string, string> assemblyNames)
    {
        var relativePath = projectRef.Replace('\\', Path.DirectorySeparatorChar);
        var relativeBase = Path.GetDirectoryName(projectPath);
        var absolutePath = Path.GetFullPath(Path.Combine(relativeBase!, relativePath));
        return assemblyNames[absolutePath];
    }
}

