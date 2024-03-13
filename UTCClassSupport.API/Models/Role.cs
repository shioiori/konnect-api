using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("roles")]
  public class Role : IdentityRole
  {
    [Column("is_global_role")]
    public bool IsGlobalRole { get; set; }
  }
}
