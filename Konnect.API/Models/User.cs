using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  public class User : IdentityUser
  {
    public string DisplayName { get; set; }
    public string Avatar { get; set; } = String.Empty;
    public virtual ICollection<Notification> Notifications { get; set; }
  }
}
