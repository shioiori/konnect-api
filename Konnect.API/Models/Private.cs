using System.ComponentModel.DataAnnotations.Schema;

namespace Konnect.API.Models
{
    [Table("private")]
    public class Private
    {
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public string Key { get; set; }
        public string Value { get; set; }
    }
}
