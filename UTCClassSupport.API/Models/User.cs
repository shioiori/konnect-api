using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("users")]
  public class User : IdentityUser
  {
    public string Name { get; set; }
  }
}
