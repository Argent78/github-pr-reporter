using GitHub.PullRequest.ConsoleApp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

// Setup services outside of ConsoleAppFramework.
var hostBuilder = Host.CreateDefaultBuilder()
    .ConfigureHostConfiguration(builder => builder.AddUserSecrets("ce7ab5bd-847b-4b11-a2a8-a9d531b42599") )
    .ConfigureServices((hostContext, services) =>
    {
        services.AddGitHubClient(hostContext);
    });

var app = ConsoleApp.CreateFromHostBuilder(hostBuilder, args);
app.AddAllCommandType();
await app.RunAsync();