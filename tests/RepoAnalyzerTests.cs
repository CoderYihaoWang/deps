using FluentAssertions;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Yihao.Deps.Tests;

public class RepoAnalyzerTests
{
    [Fact]
    public void GeneratesCorrectRepositoryJson()
    {
        var analyzer = new RepoAnalyzer();

        var repo = analyzer.Analyze(Path.Join("assets", "Sashimi.AzureCloudService"));

        repo.Should().BeEquivalentTo(expected);
    }

    private Repo expected = new("Sashimi.AzureCloudService",
        new List<Project>() 
        {
            new Project("Calamari.AzureCloudService",
                new List<string> 
                { 
                    "Calamari.Common",
                    "Calamari.AzureScripting",
                    "Calamari.Scripting",
                    "Hyak.Common",
                    "Microsoft.Azure.Common",
                    "Microsoft.WindowsAzure.Management.Compute",
                    "Microsoft.WindowsAzure.Management.Storage",
                    "Microsoft.Bcl.Async",
                    "Octopus.Dependencies.AzureBinaries",
                    "WindowsAzure.Storage"
                },
                new List<string> { }),
            new Project("Calamari.AzureCloudService.Tests",
                new List<string> 
                { 
                    "Calamari.Tests.Shared",
                    "FluentAssertions",
                    "nunit",
                    "NUnit3TestAdapter",
                    "Microsoft.NET.Test.Sdk",
                    "TeamCity.VSTest.TestAdapter",
                    "NSubstitute",
                    "System.Net.Http" },
                new List<string> 
                { 
                    "Calamari.AzureCloudService" 
                }),
            new Project("Sashimi.AzureCloudService",
                new List<string> 
                { 
                    "Portable.BouncyCastle",
                    "Sashimi.AzureScripting",
                    "Sashimi.Server.Contracts",
                    "System.IO.FileSystem.AccessControl",
                    "System.Security.Principal.Windows" 
                },
                new List<string> { }),
            new Project("Sashimi.AzureCloudService.Tests",
                new List<string> 
                { 
                    "Assent",
                    "Microsoft.NET.Test.Sdk",
                    "NUnit3TestAdapter",
                    "NSubstitute",
                    "Octopus.Server.Tests.Extensibility",
                    "Sashimi.Tests.Shared",
                    "TeamCity.VSTest.TestAdapter" 
                },
                new List<string> 
                { 
                    "Sashimi.AzureCloudService" 
                }),
        });
}