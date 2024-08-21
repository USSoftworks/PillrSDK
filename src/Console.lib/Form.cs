using System.Collections.Generic;
using Spectre.Console;

namespace Pillr.Console;

public interface IView{}

public interface IComponent
{
  public object? Output { get; }

  public IView CreateComponent(string name, object? input);
}

public class Component 
{

  /// <value>
  /// The input to the view model
  /// </value>
  public object? Input { get; set; }

  /// <value>
  /// The output of the view model
  /// </value>
  public object? Output { get; set; }

}

public class Form : Component 
{
  /// <value>
  /// A view redirected to after the form has been submitted
  /// </value>
  public View? Redirect { get; set; }

  public Form(View redirect){}
  public Form(View redirect, object redirectInput){}

  List<FormField> Fields { get; } = new List<FormField>();
}


public class FormField
{
  public string Label { get; set; }

  protected static string StyledString(string str, Style style)
  {
    return $"[{style.ToMarkup()}]{str}[/]";
  }

}

