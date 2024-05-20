using Konnect.API.Data;
using MediatR;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UpdateStateNotification
{
    public class UpdateStateNotificationCommand : UserInfo, IRequest<UpdateStateNotificationResponse>
  {
  }
}
