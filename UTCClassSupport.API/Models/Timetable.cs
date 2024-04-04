using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  [Table("timetables")]
  public class Timetable : File
  {
    public string GroupId { get; set; }
    public virtual ICollection<Shift> Shifts { get; set; }
  }
}
