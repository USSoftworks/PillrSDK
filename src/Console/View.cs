using System;
using Spectre.Console;
using Spectre.Console.Rendering;
using Pillr.Service.Client;

namespace Pillr.Console;

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
    console.Write(table);

    //var client = new PillrClient();
    //client.Connect();

    //AnsiConsole.Prompt();
    AnsiConsole.Ask<int>("Select an action: ");
  }

}
