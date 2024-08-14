using System;
using System.Collections.Generic;
using Microsoft.Build.Utilities;

namespace USSoftworks.Pillr;
public class TaskItemsFromFlatArray : Microsoft.Build.Utilities.Task
{
  [Required] public ITaskItem Configuration { get; set; }
  [Required] public string Path { get; set; }
  [Output]   public ITaskItem[] Items { get; set; }
  public int MaxLength { get; set; } = 128;
  public override bool Execute()
  {
    try
    {
      List<TaskItem> items = new List<TaskItem>();
      int i = 0;

      // 1. Take a flat array from (array.0, array.1, ...) and expand to a ITaskItem[]
      // TODO this value should be a constant somewhere in the assembly
      if(MaxLength > 4096) throw new Exception("MaxLength exceeds allowed array size");
      while(i < MaxLength)
      {
        string val = Configuration.GetMetadata($"{Path}:{i++}");
        if(String.Empty == val) break;
        items.Add(new TaskItem(val));
      }
      if(items.Count == 0) throw new Exception("Not an array");
      Items = items.ToArray();
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
    }
    return !Log.HasLoggedErrors;
  }
}
