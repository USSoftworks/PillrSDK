using Pillr.Data;
using Pillr.User;

namespace USSoftworks.Pillr;

public class UserInfo
{
  public string Name { get; set; }
  public string Email { get; set; }
  public string Phone { get; set; }
}

/// <value>
/// Open the user data store
/// The user data store is a json file stored in userprofile
/// </value>
public partial class PillrLoadUserData: Microsoft.Build.Utilities.Task
{
  [Required]
  public string PillrUserDir { get; set; }

  public UserProfile CreateUserProfile(PillrDb db)
  {

    // We need to prompt the user for information here.
    // For now we just create a dummy user
    UserInfo userInfo = new UserInfo
    {
      Name = "Ken Garrett",
      Email = "ken.garrett@ussoftworks.com",
      Phone = "+19452718382"
    };

    User? user = db.GetUserByEmail(userInfo.Email);
    // If the user email is found, ask the user is this them.
    // If so, return that user
    // If not, prompt user to use a differnt email 
    if(null == user)
    {
      Console.WriteLine("User not found, creating user record...");
      user = db.CreateUser(userInfo.Name, userInfo.Email, userInfo.Phone);
    }

    // Create the full user profile here!!
    var profile = new UserProfile
    {
      UserInfo = new UserProfile.UserInfoData
      {
        UserId = user.UserId,
        Name   = user.Name,
        Email  = user.Email,
        Phone  = user.Phone
      },
    };
    return profile;
  }

  public override bool Execute()
  {
    try
    {
      PillrDb db = BuildEngine4.GetRegisteredTaskObject(
        TaskObjectKey.PillrDb,
        RegisteredTaskObjectLifetime.AppDomain) as PillrDb;

      UserProfile? profile = PillrUser.LoadUserProfile();
      if(null == profile)
      {
        profile = CreateUserProfile(db);
        PillrUser.SaveUserProfile(profile);
      }

      //Log.LogMessage(MessageImportance.High, $"{}");
      BuildEngine4.RegisterTaskObject(
        TaskObjectKey.UserProfile, profile,
        RegisteredTaskObjectLifetime.AppDomain, false);
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
