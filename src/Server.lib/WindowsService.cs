using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Pillr.Server;
public sealed partial class WindowsService : BackgroundService
{
  private readonly ILogger<WindowsService> _logger;
  private int _executionCount = 0;

  public WindowsService(ILogger<WindowsService> logger)
  {
    _logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    try
    {
      while(!stoppingToken.IsCancellationRequested)
      {
        LogPillrService(++_executionCount);
        await Task.Delay(10000, stoppingToken);
      }
    }
    catch(Exception ex)
    {}
  }

  [LoggerMessage(
    EventId = 0,
    Level   = LogLevel.Information,
    Message = "Worker Service is up: {Count}")]
  private partial void LogPillrService(int count);
}
