using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit;

namespace GitHub.PullRequest.ConsoleApp;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddGitHubClient(this IServiceCollection services, HostBuilderContext hostContext)
    {
        services.AddScoped(_ =>
        {
            var client = new GitHubClient(new ProductHeaderValue("racwa-pull-request-stats"));
            var tokenAuth = new Credentials(hostContext.Configuration["github_token"]);
            client.Credentials = tokenAuth;
            return client;
        });
        return services;
    }
}