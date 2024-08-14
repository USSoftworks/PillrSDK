namespace Pillr.Service.Client;
public class ServerConnection 
{
  /// <value>
  /// The pipe stream used for interprocess communication with the server
  /// </value>
  public System.IO.Pipes.PipeStream Stream { get; private set; }

  /// <value>
  /// The unique id of this connection with the server
  /// </value>
  public string ConnectionId { get; private set; }

  /// <value>
  /// The name of the server
  /// </value>
  public string Name { get; private set; }

  /// <value>
  /// UTC timestamp of when the server was started
  /// </value>
  public DateTimeOffset StartTime { get; private set; }

  /// <value>
  /// UTC timestamp of when the connection was established
  /// </value>
  public DateTimeOffset ConnectTime { get; private set; }

  /// <value>
  /// The number of minutes the server will remain open listening for commands
  /// </value>
  public int TimeToLive { get; private set; }
}
