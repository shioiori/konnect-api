﻿using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("groups")]
  public class Group
  {
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    public virtual ICollection<Bulletin> Bulletins { get; set; }

  }
}
