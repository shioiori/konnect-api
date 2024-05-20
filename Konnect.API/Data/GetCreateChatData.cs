namespace Konnect.API.Data
{
  public class GetCreateChatData
  {
    public GroupData GroupData { get; set; }
    public List<UserData> Users { get; set; }
    public string CreatedBy { get; set; }
  }

  public class GroupData
  {
    public string Id { get; set; }
    public string Name { get; set; }
  }

  public class UserData
  {
    public string Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
  }
}
