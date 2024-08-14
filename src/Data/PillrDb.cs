using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Pillr.Data;
public enum MsbRuntimeType
{
  Full,
  Core,
  Mono
}

public sealed partial class PillrDb 
{
  private static readonly string ConnectionString =
  $"Data Source={Directory.GetCurrentDirectory()}\\Pillr\\Pillr.db; Pooling=false";

  private SqliteConnection? Connection { get; init; }

  private PillrDb(){}

  /// <summary>
  /// Retrieved a query string as an embedded resource use a calling functions name 
  /// </summary>
  private string GetQueryString([CallerMemberName] string? queryName = null, params string[] tokens) 
  {
    string query = null;
    try
    {
      var assembly = Assembly.GetExecutingAssembly();
      Stream stream = assembly.GetManifestResourceNames()
        .Where(resourceName => resourceName.Equals(queryName))
        .Select(resourceName => assembly.GetManifestResourceStream(resourceName))
        .Single();
      using StreamReader reader = new StreamReader(stream);
      query = String.Format(reader.ReadToEnd(), tokens);
    }
    catch(Exception ex)
    {
      throw new Exception(
        $"Could not resolve query name '{queryName}'. Either the query does not exists or there were duplicates.");
    }
    return query;
  }

  /// <summary>
  /// Create the connection to the Pillr database
  /// </summary>
  public static PillrDb? Connect(string pillrProjectDir)
  {
    string connString = $"Data Source={pillrProjectDir}\\Pillr.db; Pooling=false";
    var db = new PillrDb()
    {
      Connection = new SqliteConnection(connString)
    };
    db.Connection.Open();
    return db;
  }

  public void Close()
  {
    this.Connection.Close();
    SqliteConnection.ClearAllPools();
  }
}
