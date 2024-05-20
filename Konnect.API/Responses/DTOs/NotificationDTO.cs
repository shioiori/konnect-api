using Newtonsoft.Json.Converters;
using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Responses.DTOs
{
  public class NotificationDTO
  {
    public int Id { get; set; }
    public string Content { get; set; }
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public NotificationAction Action { get; set; }
    [Newtonsoft.Json.JsonConverter(typeof(StringEnumConverter))]
    public NotificationRange Range { get; set; }
    public object? Attach { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsSeen { get; set; }
  }
}
