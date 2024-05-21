using Konnect.ChatHub.Attributes;

namespace Konnect.ChatHub.Models
{
  [BsonCollection("Users")]
  public class User : BaseEntity
  {
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
  }
}
