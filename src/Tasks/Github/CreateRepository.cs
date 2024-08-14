using System;
using System.Linq;
using System.IO;
using Octokit;

// Github permissions
// Administration R/W
// Contents R/W
// Metadata R/O
// Webhooks R/W

namespace USSoftworks.Pillr;
public class GitHub_CreateRepo: Microsoft.Build.Utilities.Task
{
  [Required]
  public string AuthorName { get; set; }
  [Required]
  public string AuthorEmail { get; set; }
  //[Required]
  public string ApiKey { get; set; }

  public string RepoName { get; set; } = "Pillr";

  public override bool Execute()
  {
    try
    {
      GitHubClient github = new GitHubClient(new ProductHeaderValue("Pillr"));
      github.Credentials = new Octokit.Credentials(ApiKey);
      var repo = github.Repository;
      
      RepositoryVisibility visibility = RepositoryVisibility.Private;
      repo.Create(new NewRepository(RepoName)
      {
        Description = String.Empty,
        GitignoreTemplate = "CSharp",
        IsTemplate = false,
        Visibility = RepositoryVisibility.Private,
        Private = true,
        DeleteBranchOnMerge = false,
        AllowRebaseMerge = true,
        AllowSquashMerge = true,
        AllowMergeCommit = true,
        AllowAutoMerge = true,
      });
      Log.LogMessage(MessageImportance.High, $"GithubKey: {ApiKey}");
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
