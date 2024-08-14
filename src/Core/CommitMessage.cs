/// <summary>
/// Follows the conventional commits specification:
/// https://www.conventionalcommits.org/en/v1.0.0/
/// </summary>
public class CommitMessage
{
  public CommitType CommitType { get; set; }

  public bool IsBreakingChange { get; set; } = false;
  
  /// <value>
  /// An additional message about the breaking change
  /// </value>
  public string? BreakingChange { get; set; }
}

public enum CommitType
{
  Patch,         // <type> fix
  Feature,       // <type> feat
  Test,          // <type> test
  Documentation, // <type> docs
  Refactor,      // <type> refactor
  Performance,   // <type> perf
  Chore,         // <type> chore
  Style,         // <type> style
}
