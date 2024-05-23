using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using UTCClassSupport.API.Common;
using UTCClassSupport.API.Common.Mail;
using UTCClassSupport.API.Infrustructure.Data;
using UTCClassSupport.API.Infrustructure.Repositories;
using UTCClassSupport.API.Models;
using UTCClassSupport.API.Responses;
using UTCClassSupport.API.Responses.DTOs;
using UTCClassSupport.API.Utilities;

namespace UTCClassSupport.API.Application.GetNotification
{
  public class GetNotificationHandler : IRequestHandler<GetNotificationQuery, GetNotificationResponse>
  {
    private readonly NotificationManager _notificationManager;

    public GetNotificationHandler(NotificationManager notificationManager)
    {
      _notificationManager = notificationManager;
    }
    public async Task<GetNotificationResponse> Handle(GetNotificationQuery request, CancellationToken cancellationToken)
    {
      var notifications = await _notificationManager.GetNotificationAsync(request.GroupId, request.UserId, request.PaginationData);
      List<NotificationDTO> list = new List<NotificationDTO>();
      foreach (var notification in notifications)
      {
        var dto = new NotificationDTO()
        {
          Id = notification.Id,
          Content = notification.Content,
          CreatedDate = notification.CreatedDate,
          Action = notification.Action,
          Range = notification.Range,
          Attach = _notificationManager.GetAttactedObject(notification.ObjectId, notification.Action),
          IsSeen = notification.IsSeen,
        };
        list.Add(dto);
      }
      return new GetNotificationResponse()
      {
        Notifications = list
      };
      
    }
  }
}
