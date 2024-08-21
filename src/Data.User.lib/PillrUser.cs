using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Pillr.User;

public class UserProfile
{
  public UserInfoData UserInfo { get; set; }

  public class UserInfoData
  {
    [JsonPropertyName("user_id")]
    public Guid UserId { get; set; }

    [JsonPropertyName("user")]
    public string Name { get; set; }

    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("phone")]
    public string Phone { get; set; }
  }


}

public sealed partial class PillrUser
{
  private UserProfile? UserProfile { get; init; }
  public static string? HomeDir { get; set; }

  public static UserProfile? LoadUserProfile()
  {
    string homeDir = Environment.GetEnvironmentVariable("USERPROFILE");
    string jsonFile = $"{homeDir}\\Pillr\\user.json";
    if(!File.Exists(jsonFile)) return null;
    Console.WriteLine($"Loading User Profile: {jsonFile}");
    using FileStream stream = new FileStream(
      jsonFile, FileMode.Open, FileAccess.Read, FileShare.None
    );
    UserProfile profile = JsonSerializer.Deserialize<UserProfile>(stream);
    return profile;
  }

  public static UserProfile SaveUserProfile(UserProfile profile)
  {
    string homeDir = Environment.GetEnvironmentVariable("USERPROFILE");
    string jsonFile = $"{homeDir}\\Pillr\\user.json";
    Console.WriteLine($"Saving User Profile: {jsonFile}");
    using FileStream stream = new FileStream(
      jsonFile, FileMode.Create, FileAccess.Write, FileShare.None
    );
    JsonSerializer.Serialize<UserProfile>(stream, profile);
    return profile;
  }
}

