using System.Text;
using System.Text.RegularExpressions;

// https://semver.org/ 
public class SemanticVersion : IDisposable
{
  public int Major { get; init; }
  public int Minor { get; init; }
  public int Patch { get; init; }
  public string? PreRelease { get; init; }
  public string? Build { get; init; }

  /// <summary>
  /// Load a semantic version object from a SemVer 2.0 string
  /// </summary>
  public SemanticVersion(string semver)
  {
    string? core, prerelease, build;
    core = prerelease = build = null;
    try
    {
      string[] s1 = semver.Split('-', 2);
      if(s1.Length > 1)
      {
        core = s1[0];
        string[] s2 = s1[1].Split('+', 2);
        prerelease = s2[0];
        if(s2.Length > 1)
          build = s2[1];
      }
      else
      {
        string[] s2 = semver.Split('+', 2);
        if(s2.Length > 1)
        {
          core = s2[0];
          build = s2[1];
        }
        else
        {
          core = semver;
        }
      }
      string[] s3 = core.Split('.', 3);
      Major = Int32.Parse(s3[0]);
      Minor = Int32.Parse(s3[1]);
      Patch = Int32.Parse(s3[2]);
      PreRelease = prerelease;
      Build = build;
    }
    catch(Exception ex)
    {
      throw new SemanticVersionException("Failed to parse SemanticVersion", ex);
    }
  }

  //public SemanticVersion(int major, int minor, int patch, string prereleaseOrBuild){}
  public SemanticVersion(int major, int minor, int patch, string prerelease, string build)
  {
    Major = major;
    Minor = minor;
    Patch = patch; 
    PreRelease = prerelease;
    Build = build;
  }

  /// <summary>
  /// Convert a SemVer 2.0 object to a string
  /// </summary>
  public override string ToString()
  {
    StringBuilder builder = new StringBuilder($"{Major}.{Minor}.{Patch}");
    if(null != PreRelease)
      builder.Append($"-{PreRelease}");
    if(null != Build)
      builder.Append($"+{Build}");
    return builder.ToString();
  }

  /// <summary>
  /// </summary>
  public void Dispose(){}

  /// <summary>
  /// Check if string is a valid SemVer 2.0
  /// </summary>
  public static bool IsValid(string semver)
  {
    string pattern = 
      "^(0|[1-9]\\d*)\\.(0|[1-9]\\d*)\\.(0|[1-9]\\d*)" +
      "(?:-((?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*)" +
      "(?:\\.(?:0|[1-9]\\d*|\\d*[a-zA-Z-][0-9a-zA-Z-]*))*))?" +
      "(?:\\+([0-9a-zA-Z-]+(?:\\.[0-9a-zA-Z-]+)*))?$";
    return Regex.IsMatch(semver, pattern);
  }
}

public class SemanticVersionException : Exception
{
  public SemanticVersionException(string? message) : base(message){} 
  public SemanticVersionException(string? message, Exception ex) : base(message, ex){}
}
