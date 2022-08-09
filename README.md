# github-pr-reporter
Report stats on GitHub pull requests


### secret management
https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows

```
  dotnet user-secrets init
  dotnet user-secrets set "github_token" "ghp_<your_github_pat>"
```