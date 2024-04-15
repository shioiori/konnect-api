using MediatR;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.SendEmail
{
  public class SendMailCommand : BaseRequest, IRequest<SendMailResponse>
  {
    public MailContent Content { get; set; }
  }
}
