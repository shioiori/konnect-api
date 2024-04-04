﻿using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("permissions")]
  public class Permission
  {
    public string Id { get; set; }
    public int Name { get; set; }
    public virtual ICollection<Role> Roles { get; set; }
  }
}
