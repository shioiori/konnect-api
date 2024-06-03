using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Common.Mail
{
  public interface IMailHandler
  {
    Task<SendMailResponse> Send(MailContent mailContent);
  }

  public class MailHandler : IMailHandler
  {
    private readonly MailSettings _mailSettings;

    public MailHandler(MailSettings mailSettings)
    {
      _mailSettings = mailSettings;
    }

    public MailHandler(IOptions<MailSettings> mailSettings)
    {
      _mailSettings = mailSettings.Value;
    }

    public async Task<SendMailResponse> Send(MailContent mailContent)
    {
      var email = new MimeMessage();
      email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
      email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));
      email.To.Add(MailboxAddress.Parse(mailContent.To));
      email.Subject = mailContent.Subject;

      var builder = new BodyBuilder();
      builder.HtmlBody = mailContent.Body;
      email.Body = builder.ToMessageBody();

      using var smtp = new MailKit.Net.Smtp.SmtpClient();
      try
      {
        smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
        smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
        await smtp.SendAsync(email);
        return new SendMailResponse()
        {
          Success = true,
          Type = ResponseType.Success,
          Message = "Mail sent",
        };
      }
      catch (Exception ex)
      {
        throw ex;
      }
      finally
      {
        smtp.Disconnect(true);
      }
    }
  }
}
