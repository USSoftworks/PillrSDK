using EmailValidation;
using PhoneNumbers;
using Spectre.Console;
using Spectre.Console.Cli;

using Spectre.Console.Rendering;

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fasterflect;

using Pillr.Service.Client;

public class TestClass
{
  public string FirstName { get; set; }
  public string LastName { get; set; }
}

public class TableMapper<T> where T : class
{
  public Table Map(T obj)
  {
    Table table = new Table();

    IList<PropertyInfo> properties = typeof(T).Properties();
    TableColumn[] columns = properties.Select(property =>
    {
      var column = new TableColumn(property.Name);
      return column;
    }).ToArray();
    table.AddColumns(columns);

    return table;
  }
}

public class UserInfoForm
{
  private TextPrompt<string> _nameField;
  private TextPrompt<string> _emailField;
  private TextPrompt<string> _phoneField;

  private readonly IAnsiConsole _console;

  public string? Name { get; set; }
  public string? Email { get; set; }
  public string? Phone { get; set; }

  public string StyledString(string str, Style style)
  {
    return $"[{style.ToMarkup()}]{str}[/]";
  }

  public UserInfoForm(IAnsiConsole console)
  {
    Color textColor = Color.LightSkyBlue1;
    Color? textBgColor = null;
    Style style = new Style(textColor, textBgColor, Decoration.Bold);
    _console = console;


    _nameField  = new TextPrompt<string>(StyledString("Name: ", style))
      .PromptStyle(style);
    _emailField = new TextPrompt<string>(StyledString("Email: ", style))
      .PromptStyle(style)
      .Validate(emailAddressString =>
      {
        string invalidEmailMessage = "Invalid email address";
        ValidationResult result = ValidationResult.Success();
        try
        {
          if(!EmailValidator.Validate(emailAddressString))
          {
	    result = ValidationResult.Error(invalidEmailMessage);
          }
        }
        catch(Exception ex)
        {}
        return result;
      });
    _phoneField = new TextPrompt<string>(StyledString("Phone: ", style))
      .PromptStyle(style)
      .AllowEmpty()
      .Validate(phoneNumberString =>
      {
        string invalidNumberMessage = "Invalid phone number";
        ValidationResult result = ValidationResult.Success();
        if(!String.IsNullOrEmpty(phoneNumberString))
        {
          var phoneNumberUtil = PhoneNumberUtil.GetInstance();
          try
          {
            var phoneNumber = phoneNumberUtil.Parse(phoneNumberString, null);
            if(!phoneNumberUtil.IsValidNumber(phoneNumber))
            {
              result = ValidationResult.Error(invalidNumberMessage);
            }
          }
          catch(NumberParseException ex)
          {
            result = ValidationResult.Error(invalidNumberMessage);
          }
          catch(Exception ex)
          {
          }
        }
	return result;
      });
  }

  public void Show()
  {
    Name = _nameField.Show(_console);
    Email = _emailField.Show(_console);
    Phone = _phoneField.Show(_console);
  }
}


public class View
{

  public Panel GetSelectionPanel()
  {
    return new Panel("Selection")
      .BorderColor(Color.Red)
      .BorderStyle(new Style(foreground: Color.Blue, background: Color.Green, decoration: Decoration.Bold));
  }

  public void Display()
  {
	
    //SelectionPrompt<string>();
    //AnsiConsole.Clear();
    var layout = new Layout("Root")
      .SplitColumns(
        new Layout("Left"),
        new Layout("Right")
          .SplitRows(
            new Layout("Top"),
            new Layout("Bottom")
          )
      )
      .MinimumSize(5)
      .Size(10);

    layout["Left"].Update(GetSelectionPanel());

    /*
    var projectType = AnsiConsole.Prompt(
      new SelectionPrompt<string>()
      .Title("Select a project:")
      .PageSize(5)
      .HighlightStyle(new Style()
        .Background(Color.Cyan1)
        .Foreground(Color.GreenYellow)
        .Decoration(Decoration.Bold))
      .AddChoices(new[]
      {
        "Website",
        "Library",
        "Desktop",
        "Mobile",
        "Service"
      }));
    AnsiConsole.Markup($"You selected [green]{projectType}[/]");
    */

    //AnsiConsole.Write(layout);
    var console = AnsiConsole.Create(new AnsiConsoleSettings());

    /*
    var form = new UserInfoForm(console);
    form.Show();
    Console.WriteLine($"Name: {form.Name}");
    Console.WriteLine($"Email: {form.Email}");
    Console.WriteLine($"Phone: {form.Phone}");
    */

    var options = RenderOptions.Create(console);
    var obj = new TestClass();
    var mapper = new TableMapper<TestClass>();
    Table table = mapper.Map(obj);
    //table.Render(options, 32);
    console.Write(table);

    var client = new PillrClient();
    client.Connect();

    //AnsiConsole.Prompt();
    AnsiConsole.Ask<int>("Select an action: ");
  }

}
