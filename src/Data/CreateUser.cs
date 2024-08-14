using System.Data;
using Dapper;

namespace Pillr.Data;
public sealed partial class PillrDb
{
  public User CreateUser(string name, string email, string? phone)
  {
    Guid userId = Guid.NewGuid();
    return CreateUser(userId, name, email, phone);
  }

  public User CreateUser(Guid? userId, string name, string email, string? phone)
  {
    var user = Connection.QuerySingle<User>(
      GetQueryString(),
      new
      {
        UserId = userId ?? Guid.NewGuid(),
        Name = name,
        Email = email,
        Phone = phone
      });
    return user;
  }
}
