using Konnect.ChatHub.Attributes;

namespace Konnect.ChatHub.Models
{
  [BsonCollection("Groups")]

  public class Group : BaseEntity
  {
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
  }
}
