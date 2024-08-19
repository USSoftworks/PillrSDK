using Spectre.Console;

namespace Pillr.Console;

public record FormLabel(string Text, Style Style);

public class TextFormField : FormField
{
  private readonly IAnsiConsole _console;
  private TextPrompt<string> _prompt;

  private string _text;
  public string Text => _text;

  public TextFormField(IAnsiConsole console, FormLabel label)
  {
    _console = console;
    _prompt = new TextPrompt<string>(StyledString($"{label.Text}: ", label.Style))
    .PromptStyle(label.Style);
  }

  public TextFormField Show()
  {
    _text = _prompt.Show(_console);
    return this;
  }
}

public class UserInfoForm
{
  private TextFormField _nameField;
  private EmailFormField _emailField;
  private PhoneFormField _phoneField;


  public UserInfoForm(IAnsiConsole console)
  {
    Color textColor = Color.LightSkyBlue1;
    Color? textBgColor = null;
    Style style = new Style(textColor, textBgColor, Decoration.Bold);

    _nameField  = new TextFormField(console, new FormLabel("Name: ", style));
    _emailField = new EmailFormField(console, new FormLabel("Email: ", style));
    _phoneField = new PhoneFormField(console, new FormLabel("Phone: ", style));
  }

  public void Show()
  {
  }
}
