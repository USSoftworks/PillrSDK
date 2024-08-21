using PhoneNumbers;
using Spectre.Console;

namespace Pillr.Console;

public class PhoneFormField : FormField
{
  private readonly IAnsiConsole _console;
  private TextPrompt<string> _prompt;

  private PhoneNumber? _phoneNumber;
  public PhoneNumber? PhoneNumber => _phoneNumber;

  public PhoneFormField(IAnsiConsole console, FormLabel label)
  {
    _console = console;
     _prompt = new TextPrompt<string>(StyledString($"{label.Text}: ", label.Style))
    .PromptStyle(label.Style)
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
          _phoneNumber = phoneNumberUtil.Parse(phoneNumberString, null);
          if(!phoneNumberUtil.IsValidNumber(_phoneNumber))
          {
            result = ValidationResult.Error(invalidNumberMessage);
          }
        }
        catch(NumberParseException ex)
        {
          result = ValidationResult.Error(invalidNumberMessage);
        }
        catch(Exception ex)
        {}
      }
      return result;
    });
  }

  public PhoneFormField Render()
  {
    _prompt.Show(_console);
    return this;
  }
}


