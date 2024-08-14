using System.Data;
using Dapper;

namespace Pillr.Data;
public sealed partial class PillrDb 
{
  public BuildLog CreateBuildLog(Guid userId, string msbVersion, int msbRuntimeType, int cpuCount, float buildTime)
  {
    /*
    var parameters = new DynamicParameters();
    parameters.Add("@BuildId", Guid.NewGuid().ToString("N"), DbType.Guid, ParameterDirection.Input, 32);
    parameters.Add("@UserId", userId.ToString("N"), DbType.Guid, ParameterDirection.Input, 32);
    parameters.Add("@MsbVersion", msbVersion, DbType.AnsiString, ParameterDirection.Input);
    parameters.Add("@MsbRuntimeType", msbRuntimeType, DbType.Byte, ParameterDirection.Input);
    parameters.Add("@CpuCount", cpuCount, DbType.Byte, ParameterDirection.Input);
    parameters.Add("@BuildTime", buildTime, DbType.Single, ParameterDirection.Input);
    var build = Connection.QuerySingle<BuildLog>(GetQueryString(), parameters);
    */
    ///*
    var build = Connection.QuerySingle<BuildLog>(
      GetQueryString(),
      new
      {
        BuildId = Guid.NewGuid().ToString("N"),
        UserId  = userId.ToString("N"),
        MsbVersion = msbVersion,
        MsbRuntimeType = msbRuntimeType,
        CpuCount = cpuCount,
        BuildTime = buildTime
      });
    //*/
    return build;
  }
}
