using MediatR;
using UTCClassSupport.API.Requests;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UpdateStateNotification
{
  public class UpdateStateNotificationCommand : UserData, IRequest<UpdateStateNotificationResponse>
  {
  }
}
