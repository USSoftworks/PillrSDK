using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pillr.Server;
public sealed partial class PipeListener : BackgroundService
{
  private readonly ILogger<PipeListener> _logger;

  public PipeListener(ILogger<PipeListener> logger)
  {
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    Int32 maxInstances = NamedPipeServerStream.MaxAllowedServerInstances;
    PipeOptions pipeOptions =
      PipeOptions.Asynchronous | PipeOptions.CurrentUserOnly;
    using NamedPipeServerStream stream =
      new NamedPipeServerStream(
        "Pillr.Server",
        PipeDirection.InOut,
        maxInstances,
        PipeTransmissionMode.Message,
        pipeOptions);

    //while(!stoppingToken.IsCancellationRequested)
    {
      LogWaitingNamedPipeConnection();
      stream.WaitForConnection();
      LogNamedPipeConnection();
    }
    LogNamedPipeDisconnection();
    stream.Disconnect();
  }

  [LoggerMessage(
    EventId = 0,
    Level   = LogLevel.Information,
    Message = "Waiting for named pipe connection...")]
  private partial void LogWaitingNamedPipeConnection();

  [LoggerMessage(
    EventId  = 0,
    Level    = LogLevel.Information,
    Message  = "Named pipe connection made!")]
  private partial void LogNamedPipeConnection();

  [LoggerMessage(
    EventId  = 0,
    Level    = LogLevel.Information,
    Message  = "Disconnecting from pipe")]
  private partial void LogNamedPipeDisconnection();
}
