using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("timetables")]
  public class Timetable : File
  {
    [Column("group_id")]
    public string GroupId { get; set; }
    [Column("is_synchronize")]
    public bool IsSynchronize { get; set; }
    [Column("remind_time")]
    public int Remind { get; set; }
    public virtual ICollection<Event> Shifts { get; set; }
  }
}
