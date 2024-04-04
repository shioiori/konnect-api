using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("roles")]
  public class Role : IdentityRole
  {
    public virtual ICollection<Permission> Permissions { get; set; }
    public virtual ICollection<User> Users { get; set; }
  }
}
