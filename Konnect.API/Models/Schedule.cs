﻿using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("schedules")]
  public class Schedule
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

    [Column("id")]
    public string Id { get; set; }
    [Column("send_before_minutes")]
    public int SendBeforeMinutes { get; set; }
    [Column("shift_id")]
    public int ShiftId { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
    public virtual Event Shift { get; set; }
  }
}
