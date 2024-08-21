using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace USSoftworks.Pillr;
public class SendEmail : Microsoft.Build.Utilities.Task
{
  /**<value>The mailbox address of the sender</value> */    [Required] public ITaskItem? From    { get; set; }
  /**<value>A mailbox address list of the senders</value>*/ [Required] public ITaskItem[]? To    { get; set; }
  /**<value>The carbon copy address list</value>*/                     public ITaskItem[]? Cc    { get; set; }
  /**<value>The blind carbon copy address list</value>*/               public ITaskItem[]? Bcc   { get; set; }
  /**<value>The subject of the email</value>*/              [Required] public string? Subject    { get; set; }
  /**<value>The plain text body</value>*/                              public string? Text       { get; set; }
  /**<value>The html body sent</value> */                              public string? Html       { get; set; }
  /**<value>The SMTP server to use</value>*/                [Required] public string? SmtpServer { get; set; }
  /**<value>The username</value>*/                                     public string? Username   { get; set; }
  /**<value>The password</value>*/                          [Required] public string? Password   { get; set; }
  /**<value>The SMTP port. Defaults to 587</value>*/                   public short? Port        { get; set; }
  /**<value>A list of file attachments</value>*/                       public ITaskItem[]? Attachments { get; set; }

  private readonly string smtpServer = "smtp.office365.com";
  private readonly short smtpPort    = 587;

  public override bool Execute()
  {
    MailboxAddress fromAddress;
    To.Select(item =>
    {
      Log.LogMessage(Microsoft.Build.Framework.MessageImportance.High,
        $"To Address: {item.ItemSpec}");
      return item;
    });

    try
    {
      ParserOptions parseOptions = new ParserOptions
      {
        AllowAddressesWithoutDomain = false,
        AllowUnquotedCommasInAddresses = false,
        CharsetEncoding = Encoding.Unicode,
        MaxAddressGroupDepth = 16,
        MaxMimeDepth = 16,
        Rfc2047ComplianceMode = RfcComplianceMode.Strict
      };

      if(!MailboxAddress.TryParse(parseOptions, From.ItemSpec, out fromAddress))
      {
        throw new FormatException($"Bad 'From' address '{From.ItemSpec}'");
      }

      var toAddresses = To.Select(item =>
      {
        MailboxAddress address;
        if(!MailboxAddress.TryParse(parseOptions, item.ItemSpec, out address))
        {
          Log.LogWarning($"Bad 'To' Address for '{item.ItemSpec}'");
        }
        return address;
      }).ToList();
      
      MimeMessage message = new MimeMessage();
      message.From.Add(fromAddress);
      message.To.AddRange(toAddresses);
      message.Subject = Subject;
      message.Body = GetBody();

      using SmtpClient smtp = new SmtpClient(); 
      smtp.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
      smtp.Authenticate(fromAddress.Address, Password);
      smtp.Send(message);
      smtp.Disconnect(true);
    }
    catch(Exception ex)
    {
      Log.LogErrorFromException(ex);
      if(null != ex.InnerException)
        Log.LogErrorFromException(ex.InnerException);
    }
    return !Log.HasLoggedErrors;
  }

  // TODO need to add attachments here
  private MimeEntity GetBody()
  {
    var builder = new BodyBuilder();
    builder.TextBody = Text;
    builder.HtmlBody = Html;
    return builder.ToMessageBody();
  }
}

/// <summary>
/// A taskitem used to pass around mailbox addresses
/// </summary>
public class MailboxItem : ITaskItem
{
  private MailboxAddress _mailbox;
  private readonly Dictionary<string, string> _metadata;
  public MailboxItem(){}
  /*
  public MailboxItem(string? name, string address)
  {
    _mailbox = new MailboxAddress(name, address);
    _metadata = new Dictionary<string, string> {
      { "Address", address }
    };
    if(null != name) _metadata.Add("Name", name);
  }
  */
  public string ItemSpec
  {
    get { return _mailbox.ToString(null, false); }
    set { MailboxAddress.TryParse(value, out _mailbox); }
  }
  public int MetadataCount => _metadata.Count;
  public System.Collections.ICollection MetadataNames => _metadata.Keys;
  public string GetMetadata(string name)              => _metadata[name];
  public void SetMetadata(string name, string val)
  {
  }
  public void RemoveMetadata(string name)             => _metadata.Remove(name);
  public void CopyMetadataTo(ITaskItem item)          {}
  public IDictionary CloneCustomMetadata()            => new Dictionary<string, string>();
}
