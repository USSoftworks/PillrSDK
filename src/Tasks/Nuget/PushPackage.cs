using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace USSoftworks.Pillr;
public class Nuget_PushPackage : Microsoft.Build.Utilities.Task
{
  [Required] public ITaskItem[]? ServiceIndexes { get; set; } /// <value>A JSON document describing the entry point resources</value>
  [Required] public ITaskItem? PackageFile { get; set; }      /// <value>The .nupkg file being pushed</value>
  [Required] public string? ApiKey { get; set; }              /// <value>The secure API key</value>
  public ITaskItem? Certificate { get; set; }                 /// <value>The certificate (.cer) file used to sign a package</value>
  public string? SessionId { get; set; }                      /// <value>An optional client session to identify HTTP requests</value>
  public int? Version { get; set; }                           /// <value>The NuGet API version</value>
  public override bool Execute()
  {
    HttpResponseMessage response;
    CancellationToken stoppingToken = new CancellationToken(false);
    try
    {
      string nupkgFile = PackageFile.ItemSpec;
      string nupkgFileName = $"{PackageFile.GetMetadata("Filename")}{PackageFile.GetMetadata("Extension")}";
      if(!File.Exists(nupkgFile)) throw new FileNotFoundException();
      FileInfo fileInfo = new FileInfo(nupkgFile);
      Log.LogMessage(MessageImportance.High, $"NupkgFile: {nupkgFile}");

      // Setup the content as the nupkg file
      FileStream stream = new FileStream(nupkgFile, FileMode.Open);
      MultipartFormDataContent form = new MultipartFormDataContent();
      using HttpContent content = new StreamContent(stream);
      content.Headers.ContentLength = fileInfo.Length;
      //content.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
      content.Headers.ContentType = new MediaTypeHeaderValue("multipart/form-data");
      form.Add(content, "nupkg", nupkgFileName);
      using HttpClient client = new HttpClient();
      for(int i = 0; i < ServiceIndexes.Length; ++i)
      {
        // 1. GET request for the service index JSON 
        var serviceIndexUriString = ServiceIndexes[i].ItemSpec;
        //Log.LogMessage(MessageImportance.High, $"ServiceIndex: {ServiceIndexes[i].GetMetadata("Filename")}");
        if(!Uri.IsWellFormedUriString(serviceIndexUriString, UriKind.Absolute)) throw new Exception();
        Uri serviceIndexUri = new Uri(serviceIndexUriString);
        using HttpRequestMessage getRequest = new HttpRequestMessage(HttpMethod.Get, serviceIndexUri);
	response = client.Send(getRequest, stoppingToken);
        response.EnsureSuccessStatusCode();

        // 2. Parse the JSON and locate the @type PublishPackage/2.0.0 resource
        Task<ServiceIndex> requestTask = response.Content.ReadFromJsonAsync<ServiceIndex>(stoppingToken);
	requestTask.Wait();
	ServiceIndex index = requestTask.Result;
        var resource = index.Resources.First(resource => resource.Type == "PackagePublish/2.0.0");
	Uri resourceUri = new Uri(resource.Id);
	Log.LogMessage(MessageImportance.High, $"ResourceUri: {resourceUri.ToString()}");

        // 3. POST request on the @id property to push the package
        using HttpRequestMessage postRequest = new HttpRequestMessage(HttpMethod.Put, resourceUri);
        postRequest.Headers.Add("X-NuGet-Protocol-Version", "4.1.0");
        postRequest.Headers.Add("X-NuGet-ApiKey", ApiKey);
        postRequest.Content = form;
        Log.LogMessage(MessageImportance.High, "Sending .nupkg to NuGet server...");
        response = client.Send(postRequest);
        if(HttpStatusCode.NotFound == response.StatusCode)
        {
          //Uri? locationUri = response.Headers.Location;
          Task<string> responseTask = response.Content.ReadAsStringAsync(stoppingToken);
          responseTask.Wait();
	  string contentString = responseTask.Result;
          throw new Exception($"{response.StatusCode} ({response.ReasonPhrase}): {contentString}");
        }
        response.EnsureSuccessStatusCode();
        Log.LogMessage(MessageImportance.High, "File sent!");
      }
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
      if(null != ex.InnerException)
        Log.LogErrorFromException(ex.InnerException);
    }
    return !Log.HasLoggedErrors;
  }
}

public class ServiceIndex
{
  [JsonPropertyName("version")]
  public string? Version { get; set; }
  [JsonPropertyName("resources")]
  public ServiceIndexResource[]? Resources { get; set; }

  public class ServiceIndexResource
  {
    [JsonPropertyName("@id")]
    public string? Id { get; set; }
    [JsonPropertyName("@type")]
    public string? Type { get; set; }
    [JsonPropertyName("comment")]
    public string? Comment { get; set; }
  }
}
