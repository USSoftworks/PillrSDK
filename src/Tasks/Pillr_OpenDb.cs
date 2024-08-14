using Pillr.Data;

namespace USSoftworks.Pillr;

/// <value>
/// Open the Pillr database.
/// </value>
public partial class Pillr_OpenDb: Microsoft.Build.Utilities.Task
{
  [Required]
  public string PillrProjectDir { get; set; }
  public override bool Execute()
  {
    try
    {
      var db = PillrDb.Connect(PillrProjectDir);
      db.CreateSchema();
      BuildEngine4.RegisterTaskObject(
        TaskObjectKey.PillrDb, db,
        RegisteredTaskObjectLifetime.AppDomain, false);
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}


/// <value>
/// Close the Pillr database.
/// </value>
public partial class Pillr_CloseDb: Microsoft.Build.Utilities.Task
{
  public override bool Execute()
  {
    try
    {
      PillrDb db = BuildEngine4.UnregisterTaskObject
        (TaskObjectKey.PillrDb, RegisteredTaskObjectLifetime.AppDomain) as PillrDb;
      if(null != db)
      {
        db.Close();
      }
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
