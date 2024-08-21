using System.Collections.Generic;
using Dapper;

namespace Pillr.Data;
public sealed partial class PillrDb
{
  public BuildLog? GetBuildLogById(Guid buildId)
  {
    var build = Connection.QuerySingleOrDefault<BuildLog>(
      GetQueryString("GetBuildLogByX", "BuildId"),
      new { FieldValue = buildId });
    return build;
  }

  public BuildLog? GetBuildLogByDate(DateTime StartDate, DateTime? EndDate)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<BuildLog> GetAllBuildLogs()
  {
    var builds = Connection.Query<BuildLog>(GetQueryString());
    return builds;
  }
}
