using System.Linq;
using System.Net.NetworkInformation;

public class Pillr_MakeJsonFile: Microsoft.Build.Utilities.Task
{

  public string? GetMACAddress() => NetworkInterface
    .GetAllNetworkInterfaces()
    .Where(nic => nic.OperationalStatus == OperationalStatus.Up
               && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback)
    .Select(nic => nic.GetPhysicalAddress().ToString())
    .FirstOrDefault();

  public override bool Execute()
  {
    try
    {
    
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}

public class PillrSettings
{
  public Guid UserId { get; set; }
  public string Name { get; set; }
  public string Email { get; set; }
}
