using Dapper;

namespace Pillr.Data;
public sealed partial class PillrDb
{
  /// <summary>
  /// </summary>
  public int CreateSchema()
  {
    var result = Connection.Execute(GetQueryString());
    return result;
  }
}
