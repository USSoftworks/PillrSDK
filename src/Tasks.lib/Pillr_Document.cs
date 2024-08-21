using System.IO;
using System.Threading.Tasks;

public class Docfx_HtmlDoc: Microsoft.Build.Utilities.Task
{
  
  public override bool Execute()
  {
    try
    {
      string configPath = $"{Directory.GetCurrentDirectory()}\\docfx.json";
      if(!File.Exists(configPath))
      {
        //throw new Exception("Could not find docfx config file");
      }
      Task task = Docfx.Docset.Build(configPath);
      task.Wait();
      Log.LogMessage("Built HTML document!");
    }
    catch(Exception ex)
    {
      Log.LogWarningFromException(ex);
      if(null == ex.InnerException)
        Log.LogWarningFromException(ex.InnerException);
    }
    return !Log.HasLoggedErrors;
  }
}

public class Docfx_PdfDoc: Microsoft.Build.Utilities.Task
{
  public override bool Execute()
  {
    try
    {
      string configPath = $"{Directory.GetCurrentDirectory()}\\docfx.json";
      if(!File.Exists(configPath))
      {
      }
      Task task = Docfx.Docset.Pdf(configPath);
      task.Wait();
      Log.LogMessage("Built PDF document!");
    }
    catch(Exception ex)
    {
      Log.LogWarningFromException(ex);
      if(null == ex.InnerException)
        Log.LogWarningFromException(ex.InnerException);
    }
    return !Log.HasLoggedErrors;
  }
}
