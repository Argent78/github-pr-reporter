using System.Globalization;
using CsvHelper;
using GitHub.PullRequest.ConsoleApp.Models;
using Octokit;

namespace GitHub.PullRequest.ConsoleApp.Commands;

[Command("pr", "Extract data for pull requests")]
public class PullRequestApprovers : ConsoleAppBase
{
    private readonly GitHubClient gitHubClient;

    public PullRequestApprovers(GitHubClient gitHubClient)
    {
        this.gitHubClient = gitHubClient;
        // GitHub client
        // CSV Writer
        // ...?
    }

    [Command("approvers")]
    public async Task ListApproversAsync(
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
        
        // Collect the Url for each pull request
        var prIds = pullRequests
            .Select(pullRequest => pullRequest.Number)
            .ToList();

        var approvals = new List<Approval>();

        // for eachId in our collection of closed PRs
        foreach (var prId in prIds)
        {
            // get all the Approved reviews
            var prReviews = await gitHubClient.PullRequest.Review.GetAll(owner, repo, prId);
            foreach (var pullRequestReview in prReviews)
            {
                if (pullRequestReview.User.Type == AccountType.User)
                {
                    // Log the name of reviewer
                    approvals.Add(new Approval
                    {
                        PullRequestNumber = prId,
                        Username = pullRequestReview.User.Login
                    });
                }
            }
            
        }

        WriteToCsv(owner, repo, approvals);
    }

    private void WriteToCsv(string owner, string repo, List<Approval> approvals)
    {
        using var writer = new StreamWriter($"{owner}-{repo}-admin-approvals.csv");
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(approvals);
    }
    
}