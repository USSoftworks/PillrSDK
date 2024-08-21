namespace Pillr.Data;
public sealed partial class PillrDb
{
  public int UpdateProjectMetadata(Guid userId, string projectName, ProjectMetadataDelta delta)
  {
    int result = 0;
    using SqliteCommand command = Connection.CreateCommand();
    command.CommandText = GetQueryString();
    command.Parameters.Add("@UserId", SqliteType.Blob, 32).Value = userId.ToString("N");
    command.Parameters.Add("@ProjectName", SqliteType.Text).Value = projectName;
    command.Parameters.Add("@Delta", SqliteType.Integer).Value = delta;
    try
    {
      result = command.ExecuteNonQuery();
    }
    catch(SqliteException ex)
    {
      Console.WriteLine(ex.Message);
      if(null != ex.InnerException)
        Console.WriteLine($"\t{ex.InnerException.Message}");
    }
    return result;
  }
}
