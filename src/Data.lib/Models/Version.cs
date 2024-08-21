namespace Pillr.Data;
public record Version
{
  public int Major { get; set; }
  public int Minor { get; set; }
  public int Patch { get; set; }

  /// <value>
  /// The ID of the user that created the version
  /// </value>
  public Guid UserId
  {
    get { return new Guid(_UserId); }
    private set { UserId = value; }
  }
  private string _UserId { get; set; }

  /// <value>
  /// The UTC timestamp of when the version was created.
  /// </value>
  public DateTimeOffset CreatedOn
  {
    get { return DateTimeOffset.FromUnixTimeSeconds(_CreatedOn); }
    private set { CreatedOn = value; }
  }
  private Int64 _CreatedOn { get; set; }
}
