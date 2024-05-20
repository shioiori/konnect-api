using Konnect.API.Data;

namespace UTCClassSupport.API.Requests
{
    public class MesageRequest : UserInfo
  {
    public string ChatId { get; set; }
    public bool IsFile { get; set; } = false;
    public string Context { get; set; }
  }
}
