using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using System.Reflection;
using Fasterflect;

namespace Pillr.Console;
public static class TableExtensions
{
  public static Table AddData<T>(this Table table, T[] objects)
  {
    IList<PropertyInfo> properties = typeof(T).Properties(Flags.InstancePublic);
    TableColumn[] columns = properties.Select(property =>
    {
      var column = new TableColumn(property.Name);
      return column;
    }).ToArray();
    table.AddColumns(columns);
    
    foreach(T obj in objects)
    {
      string[] row = properties.Select(property =>
      {
        object? val = property.GetValue(obj);
        return val.ToString();
      }).ToArray();
      table.AddRow(row);
    }
    return table;
  }
}
