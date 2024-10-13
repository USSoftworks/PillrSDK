using Spectre.Console;
using Spectre.Console.Rendering;
using Pillr.Service.Client;
using Pillr.Utils;

namespace Pillr.Console;

public class View
{

  public Panel GetSelectionPanel(Color foregroundColor, Color backgroundColor)
  {
    Style style = new Style(
      foreground: foregroundColor,
      background: backgroundColor,
      decoration: Decoration.Bold);
    PanelHeader header = new PanelHeader("Select a task:")
      .Centered()
      .SetStyle(style);
    return new Panel("Selection")
      .Header(header)
      .BorderColor(backgroundColor)
      .BorderStyle(style);
  }

  public void Display()
  {
    //SelectionPrompt<string>();
    //AnsiConsole.Clear();
    var layout = new Layout("Root")
      .SplitColumns(
        new Layout("Left"),
        new Layout("Right"));

    layout["Left"].Update(GetSelectionPanel(Color.Red, Color.Green));
    layout["Right"].Update(GetSelectionPanel(Color.Blue, Color.Yellow));

    var console = AnsiConsole.Create(new AnsiConsoleSettings());

    var form = new UserInfoForm(console);
    form.Show();

    var options = RenderOptions.Create(console);
    var objects = new[]
    { 
      new {
        FirstName = "Jane",
        LastName = "Doe",
        Age = 36,
        CreatedOn = new DateTime(2024, 04, 05)
      },
      new {
        FirstName = "John",
        LastName = "Doe",
        Age = 40,
        CreatedOn = new DateTime(1988, 05, 12)
      }
    };

    Table table = new Table()
      .AddData<object>(objects);

    //table.Render(options, 32);
    console.Write(layout);
    console.Write(table);

    //var client = new PillrClient();
    //client.Connect();

    //AnsiConsole.Prompt();
    AnsiConsole.Ask<int>("Select an action: ");
  }

}
