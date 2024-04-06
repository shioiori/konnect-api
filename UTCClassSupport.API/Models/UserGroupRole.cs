using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("users_groups_roles")]
  public class UserGroupRole
  {
    public string UserId { get; set; }
    public string GroupId { get; set; }
    public string RoleId { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    public virtual Group Group { get; set; }
  }
}
