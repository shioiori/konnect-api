namespace UTCClassSupport.API.Requests
{
  public class InviteToGroupRequest
  {
    public string Guest { get; set; }
    public bool IsUserExist { get; set; }
  }

  public class AddGroupRequest
  {
    public string Name { get; set; }
    public bool AllowOut { get; set; }
    public bool AllowInvite { get; set; } 
  }
}
