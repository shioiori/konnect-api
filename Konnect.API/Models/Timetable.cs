using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
  public class Timetable : File
  {
    public string GroupId { get; set; }
    public bool IsSynchronize { get; set; }
    public int Remind { get; set; }
    public virtual ICollection<Event> Shifts { get; set; }
  }
}
