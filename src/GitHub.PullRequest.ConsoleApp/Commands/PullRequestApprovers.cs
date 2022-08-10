using GitHub.PullRequest.ConsoleApp.Models;
using Octokit;

// ReSharper disable UnusedMember.Global

namespace GitHub.PullRequest.ConsoleApp.Commands;

[Command("pr", "Extract data for pull requests")]
internal class PullRequestApprovers : ConsoleCommand
{
    public PullRequestApprovers(GitHubClient gitHubClient) : base(gitHubClient)
    {

    }


    [Command("approvers", "List pull request approvers")]
    public async Task ListApproversAsync(
        [Option("o", "Repository owner")] string owner,
        [Option("r", "Name of repository.")] string repo)
    {
        // Get all the closed PRs for the repo
        var pullRequests = await GetPullRequestsAsync(owner, repo);
        
        // Collect the Id for each pull request
        var prIds = pullRequests
            .Select(pullRequest => pullRequest.Number)
            .ToList();

        var approvals = new List<Contribution>();

        // for each Id in our collection of closed PRs
        foreach (var prId in prIds)
        {
            // get all the Approved reviews
            var prReviews = await GitHubClient.PullRequest.Review.GetAll(owner, repo, prId);
            foreach (var pullRequestReview in prReviews)
            {
                if (pullRequestReview.User.Type == AccountType.User &&
                    pullRequestReview.State == PullRequestReviewState.Approved)
                {
                    // Log the name of reviewer
                    approvals.Add(new Contribution
                    {
                        PullRequestNumber = prId,
                        Username = pullRequestReview.User.Login
                    });
                }
            }
        }

        WriteToCsv(owner, repo, approvals);
    }
}