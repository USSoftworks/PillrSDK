using System.Data;

namespace Pillr.Data;

[Flags]
public enum ProjectMetadataDelta
{
  None        = 0,
  ProjectName = 0x1,
  Language    = 0x2,
  All         = -1
}

public class ProjectMetadata
{
  public string ProjectName { get; init; }
  public Guid UserId { get; init; }
  public ProjectMetadataDelta Delta { get; init; }
  public DateTimeOffset CreatedOn { get; init; }
}

public sealed partial class PillrDb
{
  public ProjectMetadata? GetProjectMetadata()
  {
    ProjectMetadata metadata = null;
    using SqliteCommand command = Connection.CreateCommand();
    command.CommandText = GetQueryString();
    try
    {
      SqliteDataReader reader = command.ExecuteReader(CommandBehavior.SingleRow);
      if(reader.HasRows)
      {
        reader.Read();
        metadata = new ProjectMetadata
        {
          ProjectName = reader.GetString(0),
          UserId      = reader.GetGuid(1),
          Delta       = (ProjectMetadataDelta)reader.GetInt64(2),
          CreatedOn   = DateTimeOffset.FromUnixTimeSeconds(reader.GetInt64(3))
        };
      }
    }
    catch(SqliteException ex)
    {
      Console.WriteLine(ex.Message);
      if(null != ex.InnerException)
        Console.WriteLine($"\t{ex.InnerException.Message}");
    }
    return metadata;
  }
}
