namespace Konnect.ChatHub.Models
{
  public class User : BaseEntity
  {
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
  }
}
