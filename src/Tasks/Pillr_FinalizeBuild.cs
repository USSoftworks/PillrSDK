using Pillr.Data;
using Pillr.User;

namespace USSoftworks.Pillr;

public enum BuildHost
{
  VisualStudio,
  VSCode,
  AzureDevOps,
  GitHubAction,
  CLI
}

public class Pillr_InitializeBuild: Microsoft.Build.Utilities.Task
{
  public override bool Execute()
  {
    try
    {
    
    }
    catch(Exception ex)
    {
    
    }
    return !Log.HasLoggedErrors;
  }
}

public class ProjectMetadataDto
{
  public string ProjectName { get; set; }
  public ProjectMetadataDelta Delta { get; set; }
}

public class Pillr_FinalizeBuild: Microsoft.Build.Utilities.Task
{
  [Required] public string MsbVersion { get; set; }
  [Required] public int CpuCount { get; set; }
  [Required] public string ProjectName { get; set; }

  public override bool Execute()
  {
    try
    {
      PillrDb db = BuildEngine4.GetRegisteredTaskObject(
        TaskObjectKey.PillrDb,
        RegisteredTaskObjectLifetime.AppDomain) as PillrDb;

      UserProfile profile = BuildEngine4.GetRegisteredTaskObject(
        TaskObjectKey.UserProfile,
        RegisteredTaskObjectLifetime.AppDomain) as UserProfile;

      if(null == db) throw new Exception("Could not locate PillrDb");
      if(null == profile) throw new Exception("Could not locate PillrUser");

      if(!SemanticVersion.IsValid(MsbVersion))
      {
        Log.LogMessage(MessageImportance.High, "MSBuild Version string is invalid :(");
      }
      else
      {
        Log.LogMessage(MessageImportance.High,
          $"MSBuild Version string is valid!: {MsbVersion}");
        var semver = new SemanticVersion(MsbVersion);
      }

      // Create the build log
      var build = db.CreateBuildLog(
        profile.UserInfo.UserId, MsbVersion, 1, CpuCount, 0);


      // Check if there has been an update to the metadata,
      // if so, create a delta record
      var metadata = db.GetProjectMetadata();
      if(null == metadata)
      {
        int numRows = db.UpdateProjectMetadata(
          profile.UserInfo.UserId, ProjectName, ProjectMetadataDelta.All);
      }
      else
      {
        var dto = new ProjectMetadataDto
        {
          ProjectName = this.ProjectName,
	};
        // Check to see if there has been any changes to the metadata
        var delta = CalculateProjectMetadataDelta(metadata, dto);
        if(delta != ProjectMetadataDelta.None)
          db.UpdateProjectMetadata(
            profile.UserInfo.UserId, ProjectName, delta);
      }
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }

  public ProjectMetadataDelta CalculateProjectMetadataDelta(ProjectMetadata metadata, ProjectMetadataDto dto)
  {
    var delta = ProjectMetadataDelta.None;
    if(!dto.ProjectName.Equals(metadata.ProjectName)) delta |= ProjectMetadataDelta.ProjectName;
    return delta;
  }
}
