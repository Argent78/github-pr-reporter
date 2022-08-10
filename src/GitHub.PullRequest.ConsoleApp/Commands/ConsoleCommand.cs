using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper;
using Octokit;

namespace GitHub.PullRequest.ConsoleApp.Commands;

internal abstract class ConsoleCommand : ConsoleAppBase
{
    protected readonly GitHubClient gitHubClient;

    protected ConsoleCommand(GitHubClient gitHubClient)
    {
        this.gitHubClient = gitHubClient;
    }


    protected static void WriteToCsv<T>(
        string owner, 
        string repo, IEnumerable<T> lifetime, 
        [CallerMemberName] string? suffix = default) where T : class
    {
        var filename = $"{owner}-{repo}-{suffix?.ToLower()}.csv";
        Console.WriteLine($"Writing data to file '{filename}'");
        using var writer = new StreamWriter(filename);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(lifetime);
    }

    protected async Task<IReadOnlyList<Octokit.PullRequest>> GetPullRequestsAsync(string owner, string repo, PullRequestRequest? request = default)
    {
        request ??= new PullRequestRequest
        {
            State = ItemStateFilter.Closed
        };
        
        var pullRequests = await gitHubClient.PullRequest.GetAllForRepository(owner, repo, request);
        Console.WriteLine($"\nFetched {pullRequests.Count} pull requests for repo '{owner}/{repo}'\n");
        return pullRequests;
    }
}