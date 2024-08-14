public class Nuget_DeletePackage : Microsoft.Build.Utilities.Task
{
  public override bool Execute()
  {
    return !Log.HasLoggedErrors;
  }
}
