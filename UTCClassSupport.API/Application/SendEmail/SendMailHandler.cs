using MailKit.Security;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.SendEmail
{
  public class SendMailHandler : IRequestHandler<SendMailCommand, SendMailResponse>
  {
    private readonly MailSettings _mailSettings;
    private readonly EFContext _dbContext;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<Role> _roleManager;

    public SendMailHandler(IOptions<MailSettings> mailSettings,
      EFContext dbContext,
      UserManager<User> userManager,
        RoleManager<Role> roleManager)
    {
      _mailSettings = mailSettings.Value;
      _dbContext = dbContext;
      _userManager = userManager;
      _roleManager = roleManager;
    }
    public async Task<SendMailResponse> Handle(SendMailCommand request, CancellationToken cancellationToken)
    {
      var mailContent = request.Content;
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
          Message = "Mail sent",
        };
      }
      catch (Exception ex)
      {
        return new SendMailResponse()
        {
          Success = false,
          Message = ex.Message,
        };
      }
      finally
      {
        smtp.Disconnect(true);
      }
    }
  }
}
