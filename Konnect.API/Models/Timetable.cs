using System.ComponentModel.DataAnnotations.Schema;

namespace UTCClassSupport.API.Models
{
	[Table("timetables")]
	public class Timetable
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Column("id")]
		public string Id { get; set; }
		[Column("is_synchronize")]
		public bool IsSynchronize { get; set; }
		[Column("remind_time")]
		public int Remind { get; set; }
		[Column("created_by")]
		public string CreatedBy { get; set; }
		public virtual ICollection<Event> Shifts { get; set; }
	}
}
