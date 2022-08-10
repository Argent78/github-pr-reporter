using GitHub.PullRequest.ConsoleApp.Models;
using Octokit;

namespace GitHub.PullRequest.ConsoleApp.Commands;

[Command("pr", "Extract data for pull requests")]
internal class PullRequestCreator : ConsoleCommand
{
    public PullRequestCreator(GitHubClient gitHubClient) : base(gitHubClient)
    {
            
    }


    [Command("creators", "List pull request creators")]
    public async Task ListCreatorsAsync(
        [Option("o", "Repository owner")] string owner,
        [Option("r", "Name of repository.")] string repo)
    {
        // Get all the closed PRs for the repo
        var pullRequests = await GetPullRequestsAsync(owner, repo);

        var creators = new List<Contribution>();
        foreach (var pullRequest in pullRequests)
        {
            var creator = pullRequest.User;
            creators.Add(new Contribution {
                PullRequestNumber   = pullRequest.Number,
                Username = creator.Login
            });
        }

        WriteToCsv(owner, repo, creators);
    }
}