using GitHub.PullRequest.ConsoleApp.Models;
using Octokit;

namespace GitHub.PullRequest.ConsoleApp.Commands;

[Command("pr", "Extract data for pull requests")]
internal class PullRequestLifetime : ConsoleCommand
{
    public PullRequestLifetime(GitHubClient gitHubClient) : base(gitHubClient)
    {

    }

    [Command("lifetime", "Extract pull request lifetime data")]
    public async Task ExtractLifetimeStatsAsync(
        [Option("o", "Repository owner")] string owner,
        [Option("r", "Name of repository.")] string repo)
    {
        // Get all the closed PRs for the repo
        var pullRequests = await GetPullRequestsAsync(owner, repo);

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