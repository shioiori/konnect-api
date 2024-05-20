using UTCClassSupport.API.Responses.DTOs;

namespace UTCClassSupport.API.Responses
{
  public class GetNotificationResponse : Response
  {
    public List<NotificationDTO> Notifications { get; set; }
  }

  public class UpdateStateNotificationResponse : Response { }
}
