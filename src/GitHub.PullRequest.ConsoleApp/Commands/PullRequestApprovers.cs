
namespace GitHub.PullRequest.ConsoleApp.Commands;

[Command("pr")]
public class PullRequestApprovers : ConsoleAppBase, IAsyncDisposable
{

    [Command("approvers")]
    public async Task ListApproversAsync(
        [Option("o", "Repository owner")] string owner,
        [Option("r", "Name of repository.")] string repo)
    {
        Console.WriteLine($"{owner} - {repo}");
    }


    public ValueTask DisposeAsync()
    {
        return ValueTask.CompletedTask;
    }
}