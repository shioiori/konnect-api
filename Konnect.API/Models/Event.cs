using System.ComponentModel.DataAnnotations.Schema;
using UTCClassSupport.API.Common;

namespace UTCClassSupport.API.Models
{
  public class Event
  {
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string TimetableId { get; set; }
    /// <summary>
    /// day in week (mon, tue, vv...)
    /// </summary>
    public int? Day { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public bool IsLoopPerDay { get; set; }
    public TimeSpan? PeriodStart { get; set; }
    public TimeSpan? PeriodEnd { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string? Location { get; set; }
    public string? Subject { get; set; }
    public string? SubjectCode { get; set; }
    public string? SubjectClass { get; set; }
    public int? Credit { get; set; }
    public EventCategory Category { get; set; } = EventCategory.Unclassified;
    public virtual Timetable Timetable { get; set; }
  }
}
