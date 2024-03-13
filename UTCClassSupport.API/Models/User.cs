using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("users")]
  public class User : IdentityUser
  {
    [Column("group_id")]
    public int GroupId { get; set; }
    public int GroupRole { get; set; }
  }
}
