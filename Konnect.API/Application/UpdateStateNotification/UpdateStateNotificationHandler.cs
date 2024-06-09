using MediatR;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Responses;

namespace UTCClassSupport.API.Application.UpdateStateNotification
{
  public class UpdateStateNotificationHandler : IRequestHandler<UpdateStateNotificationCommand, UpdateStateNotificationResponse>
  {
    private readonly NotificationManager _notificationManager;
    public UpdateStateNotificationHandler(NotificationManager notificationManager)
    {
      _notificationManager = notificationManager;
    }
    public Task<UpdateStateNotificationResponse> Handle(UpdateStateNotificationCommand request, CancellationToken cancellationToken)
    {
      _notificationManager.UpdateStateNotification(request.GroupId, request.UserId, request.NotificationId);
      return Task.FromResult(new UpdateStateNotificationResponse()); 
    }
  }
}
