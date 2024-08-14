using System;
using System.IO;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using DotNetEnv;

namespace USSoftworks.Pillr;
public class Kudu_DeployZip : Microsoft.Build.Utilities.Task
{
  /** <value>The name of the azure web service</value> */                  [Required] public string? Sitename   { get; set; }
  /** <value>The username in azure</value>*/                               [Required] public string? Username   { get; set; }
  /** <value>The password</value>*/                                        [Required] public string? Password   { get; set; }
  /** <value>The zip archive containing the deployment contents</value> */ [Required] public ITaskItem? ZipFile { get; set; }
  /** <value>Perform the deployment asynchronously</value> */                         public bool IsAsync       { get; set; }
  /** <value>A pollable uri returned for status updates</value> */         [Output]   public string? StatusUri  { get; set; }

  private readonly string _uristring = "https://{0}.scm.azurewebsites.net/api/zipdeploy";
  // App Service Environment - https://{0}.scm.{1}.p.azurewebsites.net
  //ASE for internal load balancing - https://{0}.scm.{1}.appserviceenvironment.net

  public Kudu_DeployZip() => IsAsync = false;

  public override bool Execute()
  {
    // TODO Need to validate that these exists
    //string username = ConfigurationManager.GetMetadata("KuduUsername");
    //string password = ConfigurationManager.GetMetadata("KuduPassword");
    //string sitename = ConfigurationManager.GetMetadata("KuduSitename");
    Log.LogMessage(MessageImportance.High, Sitename);
    Log.LogMessage(MessageImportance.High, Username);
    Log.LogMessage(MessageImportance.High, Password);
    try
    {
      //string password = "Pr9JHcR1txj2ygDCsBPH!";
      string zipFile = ZipFile.ItemSpec;
      if(!File.Exists(zipFile)) throw new FileNotFoundException();

      FileInfo fileInfo = new FileInfo(zipFile);
      Uri uri = new Uri(String.Format(_uristring, Sitename));
      Log.LogMessage(MessageImportance.High, uri.ToString());
      //if(IsAsync)  Append to query
      var authToken = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{Username}:{Password}"));
      FileStream stream = new FileStream(zipFile, FileMode.Open);
      using HttpClient client = new HttpClient();
      using HttpContent content = new StreamContent(stream);
      content.Headers.ContentLength = fileInfo.Length;
      content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
      var request = new HttpRequestMessage(HttpMethod.Post, uri);
      request.Headers.Authorization = new AuthenticationHeaderValue("Basic", authToken);
      request.Content = content;
      Log.LogMessage(MessageImportance.High, "Sending zip file to kudu...");
      HttpResponseMessage response;
      if(IsAsync)
      {
        System.Threading.Tasks.Task<HttpResponseMessage> task = client.SendAsync(request);
        task.Wait();
        response = task.Result;
        //if(IsAsync) response.Headers.Location
      }
      else
      {
        //response = client.Send(request);
      }
      //response.EnsureSuccessStatusCode();
      Log.LogMessage(MessageImportance.High, "File sent!");
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
      Log.LogWarning("Looking for inner exception...");
      if(null != ex.InnerException)
        Log.LogErrorFromException(ex.InnerException);
    }
    return !Log.HasLoggedErrors;
  }
}

public static class StreamExtensions
{
  public static byte[] ReadAllBytes(this Stream inStream)
  {
    if(inStream is MemoryStream)
      return (inStream as MemoryStream).ToArray();
    using(var memStream = new MemoryStream())
    {
      inStream.CopyTo(memStream);
      return memStream.ToArray();
    }
  }
}
