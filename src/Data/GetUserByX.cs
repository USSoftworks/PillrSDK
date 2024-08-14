using Dapper;

namespace Pillr.Data;
public sealed partial class PillrDb
{
  public User? GetUserById(Guid userId)
  {
    var user = Connection.QuerySingleOrDefault<User>(
      GetQueryString("GetUserByX", "UserId"),
      new { Value = userId });
    return user;
  }

  public User? GetUserByEmail(string email)
  {
    var user = Connection.QuerySingleOrDefault<User>(
      GetQueryString("GetUserByX", "Email"),
      new { Value = email });
    return user;
  }
}
