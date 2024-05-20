using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.SendEmail
{
    public class SendMailCommand : UserInfo, IRequest<SendMailResponse>
  {
    public MailContent Content { get; set; }
  }
}
