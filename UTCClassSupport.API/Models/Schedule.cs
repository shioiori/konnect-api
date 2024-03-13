using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("schedules")]
  public class Schedule
  {
    [Column("id")]
    public int Id { get; set; }
    [Column("send_before_minutes")]
    public int SendBeforeMinutes { get; set; }
    [Column("created_by")]
    public string CreatedBy { get; set; }
  }
}
