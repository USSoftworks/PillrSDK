using System.Collections.Generic;

namespace ConvCommit;

public record CommitFooter(string Token, string Description);

public class CommitWriterOptions
{
  public int MaximumLineLength { get; set; } = 80;
}

/// <summary>
/// Follows the conventional commits specification:
/// https://www.conventionalcommits.org/en/v1.0.0/
/// </summary>
public class ConventionalCommit 
{
  public ConventionalCommit(string type, string description)
  {
    Type = type;
    Description = description;
  }
  
  public CommitType CommitType { get; set; }
  public string Type { get; set; }

  public string? Scope { get; set; }

  public string Description { get; set; }


  public bool IsBreakingChange { get; private set; } = false;

  /// <value>The number of footers in the commit message.</value>
  public int FooterCount => Footers.Count;

  /// <value>The number of paragraphs in the commit message.</value>
  public int ParagraphCount => Body.Count;


  public List<string> Body { get; } = new List<string>();

  public List<CommitFooter> Footers { get; } = new List<CommitFooter>();


  /// <summary>Denotes that this commit is a breaking change.</summary>
  public void SetBreakingChange()
  {
    IsBreakingChange = true;
  }

  /// <summary>Denotes that this commit is a breaking change with a description that gets added as footer.</summary>
  public void SetBreakingChange(string description)
  {
    Footers.Add(new CommitFooter("BREAKING CHANGE", description));
    IsBreakingChange = true;
  }

  /// <summary>Add a footer to the commit with a description.</summary>
  public void AddFooter(string token, string description)
  {
    Footers.Add(new CommitFooter(token, description));
  }

  ///<summary>Writes a conventional commit out to its properly formatted string.</summary>
  public override string ToString()
  {
    var builder = new System.Text.StringBuilder();
    if(String.IsNullOrEmpty(Scope))
      builder.AppendFormat("{0}{1}: {2}", Type, IsBreakingChange ? "!" : "", Description);
    else
      builder.AppendFormat("{0}({1}){2}: {3}", Type, Scope, IsBreakingChange ? "!" : "", Description);
    foreach(string paragraph in Body)
    {
      builder.AppendLine();
      builder.Append(paragraph);
    }
    if(Footers.Count > 0)
    {
      builder.AppendLine();
      foreach(var footer in Footers)
      {
        builder.AppendLine($"{footer.Token}: {footer.Description}");
      }
    }
    return builder.ToString();
  }

  /// <summary>Parse a conventional commit message. </summary>
  public static ConventionalCommit Parse(string commitMessage)
  {
    string type = null;
    string description = null;

    var commit = new ConventionalCommit(type, description);
    return commit;
  }
}


