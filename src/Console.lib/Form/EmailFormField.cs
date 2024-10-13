using EmailValidation;
using MimeKit;
using Spectre.Console;

namespace Pillr.Console;

public class EmailFormField : FormField
{
  private readonly IAnsiConsole _console;
  private TextPrompt<string> _prompt;

  private MailboxAddress? _mailboxAddress;
  public MailboxAddress MailboxAddress => _mailboxAddress;

  public EmailFormField(IAnsiConsole console, FormLabel label)
  {
    _console = console;
    _prompt = new TextPrompt<string>(StyledString($"{label.Text}: ", label.Style))
    .PromptStyle(label.Style)
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
  }

  public EmailFormField Render()
  {
    string email = _prompt.Show(_console);
    _mailboxAddress = new MailboxAddress(null, email);
    return this;
  }
}


