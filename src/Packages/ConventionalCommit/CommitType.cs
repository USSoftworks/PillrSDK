namespace ConvCommit;

public enum CommitType
{
  /// <value> fix: A bug fix </value>
  Patch,
  /// <value> feat: A new feature </value>
  Feature,
  /// <value> test: Adding missing tests or correcting existing tests </value>
  Test,
  /// <value> docs: Documentation only changes </value>
  Documentation,
  /// <value> refactor: A code change that neither fixes a bug nor adds a feature </value>
  Refactor,
  /// <value> perf: A code change that improves performance </value>
  Performance,
  /// <value> chore: </value>
  Chore,
  /// <value> style: Changes that do not affect the meaning of the code
  /// (white-space, formatting, missing semi-colons, etc.)
  /// </value>
  Style
}
