using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("users_groups_roles")]
  public class UserGroupRole
  {
    [Column("user_id")]
    public string UserId { get; set; }
    [Column("group_id")]
    public string GroupId { get; set; }
    [Column("role_id")]
    public string RoleId { get; set; }
    public virtual User User { get; set; }
    public virtual Role Role { get; set; }
    public virtual Group Group { get; set; }
  }
}
