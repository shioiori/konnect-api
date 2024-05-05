using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("users")]
  public class User : IdentityUser
  {
    public string Name { get; set; }
    public string Avatar { get; set; }
    public virtual ICollection<Chat> Chats { get; set; }
  }
}
