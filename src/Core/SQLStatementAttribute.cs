using System;

namespace Pillr.Shared;
[AttributeUsage(AttributeTargets.Field)]
public class SQLStatementAttribute : Attribute
{
  public string FileName { get; set; }
  public string PropName { get; set; }
  public SQLStatementAttribute(string fileName, string propName)
  {
    FileName = fileName;
    PropName = propName;
  }
}
