using System.Threading;
using System.Threading.Tasks;
using SlackNet;
using SlackNet.WebApi;

// For instructions on how to create a slack bot token: https://api.slack.com/tutorials/tracks/getting-a-token 
namespace USSoftworks.Pillr;
public class Slack_PostMessage : Microsoft.Build.Utilities.Task
{
  /// <value>The Slack APIKey to use.</value>
  [Required] public string ApiKey { get; set; }

  /// <value>The Slack channel post a message on.</value>
  [Required] public string Channel { get; set; }

  /// <value>The message text</value>
  public string Text { get; set; }

  /// <value>The stopping token</value>
  public CancellationToken StoppingToken { get; set; }

  /// <value>Throw error if Slack returns an error. Warning by default.</value>
  public bool ThrowError { get; set; } = false;


  private string _botName = "Pillr";

  public override bool Execute()
  {
    var StoppingToken = new CancellationToken();
    try
    {
      // 1. Send slack message
      Log.LogMessage(MessageImportance.High, $"Sending Slack Message...");
      var slackApi = new SlackServiceBuilder()
        .UseApiToken(ApiKey)
        .GetApiClient();
      Task<PostMessageResponse> task = slackApi.Chat.PostMessage(new Message
      {
        Channel = Channel,
        Text = Text
      }, StoppingToken);
      task.Wait();
      PostMessageResponse response = task.Result;
      Log.LogMessage(MessageImportance.High, $"Response: {response.Ts} | {response.Channel}");
  
      // 2. Store Slack message in database
      
    }
    catch(Exception ex)
    {
      if(ThrowError)
        Log.LogErrorFromException(ex);
      else
        Log.LogWarningFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
