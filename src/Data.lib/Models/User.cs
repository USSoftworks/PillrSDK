namespace Pillr.Data;
public record User
{
  /// <value>
  /// The ID of the user
  /// </value>
  public Guid UserId
  {
    get { return new Guid(_UserId); }
    private set { UserId = value; }
  }
  private string _UserId { get; set; }

  /// <value>
  /// The name of the user.
  /// </value>
  public string Name { get; set; }

  /// <value>
  /// The email of the user.
  /// </value>
  public string Email { get; set; } 

  /// <value>
  /// The phone number of the user
  /// </value>
  public string Phone { get; set; }

  /// <value>
  /// The UTC timestamp of when the user was created
  /// </value>
  public DateTimeOffset CreatedOn
  {
    get { return DateTimeOffset.FromUnixTimeSeconds(_CreatedOn); }
    private set { CreatedOn = value; }
  }
  private Int64 _CreatedOn { get; set; }
}


