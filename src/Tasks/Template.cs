using System;
using Microsoft.Build.Framework;

public class CLASSNAME : Microsoft.Build.Utilities.Task
{

  public override bool Execute()
  {
    try
    {
      
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
