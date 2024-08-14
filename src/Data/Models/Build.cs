namespace Pillr.Data;
public record BuildLog
{
  /// <value>
  /// The ID of the build
  /// </value>
  public Guid BuildId
  {
    get { return new Guid(_BuildId); }
    private set { BuildId = value; }
  }
  private string _BuildId { get; set; }


  /// <value>
  /// The ID of the user that created the build
  /// </value>
  public Guid UserId
  {
    get { return new Guid(_UserId); }
    private set { UserId = value; }
  }
  private string _UserId { get; set; }


  /// <value>
  /// The version of MSBuild that was used to create the build.
  /// </value>
  public string MSBuildVersion { get; set; }


  /// <value>
  /// The runtime type of MSBuild that was used to create the build.
  /// </value>
  public string MSBuildRuntimeType { get; set; }


  /// <value>
  /// The number of CPU nodes used while building.
  /// </value>
  public int CpuCount { get; set; }


  /// <value>
  /// The amount of time in milliseconds that it took to build
  /// </value>
  public long BuildTime { get; set; }


  /// <value>
  /// The UTC timestamp of when the build was created
  /// </value>
  public DateTimeOffset CreatedOn
  {
    get { return DateTimeOffset.FromUnixTimeSeconds(_CreatedOn); }
    private set { CreatedOn = value; }
  }
  private Int64 _CreatedOn { get; set; }
}
