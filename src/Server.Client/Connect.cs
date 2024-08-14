using System.Linq;
using System.IO;
using System.IO.Pipes;
using System.Reflection;
using System.Security.Principal;
using Microsoft.Extensions.Logging;

namespace Pillr.Service.Client;

public sealed partial class PillrClient
{
  internal static readonly string PillrServerPath = Path.Combine(
    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
    "Pillr.Server.exe"
  );

  private readonly string _pipeName = "Pillr.Server";
  private bool IsServerRunning()
  {
    return System.IO.Directory
      .GetFiles("\\\\.\\pipe\\")
      .Contains($"\\\\.\\pipe\\{_pipeName}");
  }

  public void StartServer()
  {
    
    Console.WriteLine($"Starting the server from: {PillrServerPath}");
    /*
    var process = new Process
    {
      //StartInfo.FileName = 
      StartInfo.UseShellExecute = true,
    }
    */
  }

  public ServerConnection Connect()
  {
    Span<byte> readSpan = stackalloc byte[256];
    PipeOptions pipeOptions =
      PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly;
    try
    {
      if(!IsServerRunning())
      {
        StartServer();
      }
      NamedPipeClientStream stream =
        new NamedPipeClientStream(
          ".",
          _pipeName,
          PipeDirection.InOut,
          pipeOptions,
          TokenImpersonationLevel.None);
      stream.Connect(5000);
      // read the connection packet
      int numBytes = stream.Read(readSpan);
    }
    catch(TimeoutException timeoutException)
    {
      //LogPillrClientFailed(timeoutException);
      Console.WriteLine("Failed with Exception");
    }
    return new ServerConnection();
  }

  /*
  [LoggerMessage(
    EventId  = 0,
    Level    = LogLevel.Information,
    Message  = "Failed with Exception {ex}")]
  private partial void LogPillrClientFailed(ILogger logger, Exception ex);
  */
}
