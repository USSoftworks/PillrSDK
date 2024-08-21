using System;
using System.Data.SqlClient;
using Microsoft.Build.Framework;
using Microsoft.Data.Sqlite;

public class QueryPillrDb : Microsoft.Build.Utilities.Task
{
  [Required]
  public string ConnectionString { get; set; }

  public override bool Execute()
  {
    try
    {
      using (SqliteConnection conn = new SqliteConnection(ConnectionString))
      {
        conn.Open();
        string sql =
        @"";
        using var command = new SqliteCommand(sql, conn);
	using var reader = command.ExecuteReader();
        if(reader.HasRows)
        {
          //while()
        }
      }
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
