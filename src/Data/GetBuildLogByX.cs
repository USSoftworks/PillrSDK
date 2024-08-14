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
}
