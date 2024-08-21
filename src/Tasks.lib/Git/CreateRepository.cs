using System;
using System.IO;
using LibGit2Sharp;

namespace USSoftworks.Pillr;
public class Git_CreateRepo: Microsoft.Build.Utilities.Task
{
  [Required]
  public string AuthorName { get; set; }
  [Required]
  public string AuthorEmail { get; set; }

  private string commitMessage = "Initial commit";

  private string repoName = "";

  public enum VersionControlStrategy
  {
    Gitflow,
    Trunk
  }

  public override bool Execute()
  {
    try
    {
      // 1. Create local git repository
      string workingDir = Repository.Init(Directory.GetCurrentDirectory());
      using Repository repo = new Repository(workingDir);
      // TODO We need to prompt who the person is before initial commit
      Identity id = new Identity(AuthorName, AuthorEmail);
      var sign = new Signature(id, DateTimeOffset.UtcNow);
      CommitOptions opts = new CommitOptions
      {
        AmendPreviousCommit = false,
        AllowEmptyCommit = true,
        CommentaryChar = '#'
      };
      repo.Commit(commitMessage, sign, sign, opts);

     /*
      if(null == repo.Branches["master"]) {
        repo.CreateBranch("master");
      }
      else {
        Log.LogMessage(MessageImportance.High, $"Branch 'master' already exists");
      }
      */
      //repo.CreateBranch("prod");
      //Log.LogMessage(MessageImportance.High, $"Branch Count: {repo.Branches.Count()}");
      /*
      repo.Branches.Select(b =>
      {
        Log.LogMessage(MessageImportance.High, $"Branch: {b.CanonicalName}");
	return b;
      });
      */
      //repo.branches.Add("dev");
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
