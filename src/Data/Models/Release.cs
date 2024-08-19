using System.Collections.Generic;
using System.Linq;

namespace Pillr.Data;
public class Release
{
  /// <value>
  /// The identifier of the release
  /// </value>
  public Guid ReleaseId
  {
    get { return new Guid(_ReleaseId); } 
    private set { ReleaseId = value; }
  }
  private string _ReleaseId { get; set; }

  /// <value>
  /// The ID of the build record that this release is from
  /// </value>
  public Guid BuildId
  {
    get { return new Guid(_BuildId); }
    private set { _BuildId = value.ToString("N"); }
  }
  private string _BuildId { get; set; }


  public DateTimeOffset CreatedOn 
  {
    get
    {
      return DateTimeOffset.FromUnixTimeSeconds(_CreatedOn);
    }
  }
  private Int64 _CreatedOn { get; set; }


  public List<ReleaseNotes> ReleaseNotes
  {
    get { return _ReleaseNotes.ToList(); }
  }
  private IEnumerable<ReleaseNotes> _ReleaseNotes { get; set; }
}

public class ReleaseNotes
{
  /// <value>The identifier of the user that created the note</value>
  public Guid UserId
  {
    get { return new Guid(_UserId); }
    private set { _UserId = value.ToString("N"); }
  }
  private string _UserId { get; set; }


  /// <value>The note</value>
  public string Note { get; set; }


  /// <value>The UTC offset time the note was created</value>
  public DateTimeOffset CreatedOn
  {
    get { return DateTimeOffset.FromUnixTimeSeconds(_CreatedOn); }
  }
  private Int64 _CreatedOn { get; set; }

}
