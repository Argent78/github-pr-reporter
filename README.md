# github-pr-reporter
Report stats on GitHub pull requests

# Overview
TODO: this bit.


# Secret management
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows

```
  dotnet user-secrets init
  dotnet user-secrets set "github_token" "ghp_<your_github_pat>"
```


# Running commands 
In launchSettings.json

```json
{
  "profiles": {
    "GitHub.PullRequest.Console": {
      "commandName": "Project",
      "commandLineArgs": "pr approvers -o racwa -r github-repo-management"
    }
  }
}
```

```
"commandLineArgs": "pr approvers -o racwa -r github-repo-management"
"commandLineArgs": "pr lifetime -o racwa -r github-repo-management"
```