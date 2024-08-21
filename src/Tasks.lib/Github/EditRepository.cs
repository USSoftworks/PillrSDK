using System;
using Microsoft.Build.Framework;

namespace USSoftworks.Pillr;
public class GitHub_EditRepository: Microsoft.Build.Utilities.Task
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
