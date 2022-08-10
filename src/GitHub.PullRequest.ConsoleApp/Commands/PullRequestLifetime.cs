using GitHub.PullRequest.ConsoleApp.Models;
using Octokit;

namespace GitHub.PullRequest.ConsoleApp.Commands;

[Command("pr", "Extract data for pull requests")]
internal class PullRequestLifetime : ConsoleCommand
{
    private readonly GitHubClient gitHubClient;

    public PullRequestLifetime(GitHubClient gitHubClient)
    {
        this.gitHubClient = gitHubClient;
    }

    [Command("lifetime", "Extract pull request lifetime data")]
    public async Task ExtractLifetimeStatsAsync(
        [Option("o", "Repository owner")] string owner,
        [Option("r", "Name of repository.")] string repo)
    {
        // Get all the closed PRs for the repo
        // https://api.github.com/repos/racwa/github-repo-management/pulls?state=closed
        var request = new PullRequestRequest
        {
            State = ItemStateFilter.Closed
        };


        var pullRequests = await gitHubClient.PullRequest.GetAllForRepository(owner, repo, request);
        Console.WriteLine($"\nFetched {pullRequests.Count} closed pull requests for repo '{owner}/{repo}'\n");
        
        var prLifetimes = new List<Lifetime>();

        foreach (var pullRequest in pullRequests)
        {
            if (!pullRequest.Merged) continue; // if not merged then skip it

            var createdAt = pullRequest.CreatedAt;
            var closedAt = pullRequest.ClosedAt;

            var lifetime = closedAt - createdAt;
            if (lifetime == null) continue;

            prLifetimes.Add(new Lifetime
            {
                PullRequestNumber = pullRequest.Number,
                TotalHours = lifetime.Value.TotalHours
            });
        }
        
        WriteToCsv(owner, repo, prLifetimes);
    }

}