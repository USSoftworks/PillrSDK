using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Utilities;
using Microsoft.Extensions.Configuration;

namespace USSoftworks.Pillr;
public class Configure : Microsoft.Build.Utilities.Task
{
  public ITaskItem? EnvFile { get; set; }
  public bool Overwrite { get; set; } = false;
  [Output] public ITaskItem Configuration { get; set; }

  private string? Environment =>
       System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT")
    ?? System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

  public override bool Execute()
  {
    string curDir = System.IO.Directory.GetCurrentDirectory();
    try
    {
      if(null == EnvFile)
      {
        DotNetEnv.Env.Load();
        DotNetEnv.Env.TraversePath().Load();
      }
      else
      {
        if(File.Exists(EnvFile.ItemSpec))
        {
          DotNetEnv.Env.Load(EnvFile.ItemSpec);
        }
      }
      Configuration = new TaskItem("Configuration");
      new ConfigurationManager()
        .AddEnvironmentVariables()
        .AddJsonFile($"{curDir}\\appsettings.json", true)
        .AddJsonFile($"{curDir}\\appsettings.{Environment}.json", true)
        .Build()
        .AsEnumerable()
        .Select(kv =>
        {
          Configuration.SetMetadata(kv.Key, kv.Value);
          return kv;
        })
        .ToList();
    }
    catch(System.Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
