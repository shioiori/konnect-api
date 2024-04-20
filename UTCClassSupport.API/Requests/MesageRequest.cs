namespace UTCClassSupport.API.Requests
{
  public class MesageRequest : UserData
  {
    public string ChatId { get; set; }
    public bool IsFile { get; set; } = false;
    public string Context { get; set; }
  }
}
