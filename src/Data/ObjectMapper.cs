using System.Collections.Generic;
using System.Reflection;
using Fasterflect;

public static class ObjectMapper
{
  public static void Map<T>()
  {
    // 1. Get all the properties on a record
    IList<PropertyInfo> properties = typeof(T).Properties();
  }
}
