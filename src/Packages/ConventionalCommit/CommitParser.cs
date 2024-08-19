using System;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;

namespace ConvCommit;

[Flags]
public enum StandardCommitParserOptions
{
  Strict = 0,
  Allow
}

internal static class StandardCommitParser
{
  public static IEnumerable<Exception> Parse(string commitMessage)
  {
    var exceptions = new List<Exception>();
    // 1. Parse out whitespace
   
    // 2. Parse up to the first colon
    
    return exceptions;
  }

  private static Exception? ParseHeader(
    ref StringSegment messageSegment,
    out StringSegment prefixSegment,
    out StringSegment summarySegment)
  {
    Exception? ex = null;
    prefixSegment = null;
    summarySegment = null;
    char[] ch = {'\r', '\n'};
    int index = messageSegment.IndexOfAny(ch);
    return ex;
  }

  private static Exception? ParsePrefix(
    StringSegment headerSegment,
    out StringSegment prefixSegment,
    out StringSegment descriptionSegment)
  {
    Exception ex = null;
    descriptionSegment = null;
    // parse up to the first colon
    int index = headerSegment.IndexOf(':');

    prefixSegment = headerSegment.Subsegment(index);
    return ex;
  }
}
