using Pillr.Data;

namespace Pillr.Console;

public class BuildDisplayTableFilters
{
  /// <value>
  /// The date to filter out builds
  /// </value>
  public DateTime? StartDate { get; set; }

  /// <value>
  /// Use this date with StartDate to provide a range
  /// </value>
  public DateTime? EndDate { get; set; }
}

public class BuildDisplayTable
{
  public void GetData()
  {
    
  }
}
