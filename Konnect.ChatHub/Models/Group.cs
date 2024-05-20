namespace Konnect.ChatHub.Models
{
  public class Group : BaseEntity
  {
    public string Name { get; set; }
    public ICollection<User> Users { get; set; }
  }
}
