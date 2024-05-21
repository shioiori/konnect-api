using Konnect.ChatHub.Attributes;

namespace Konnect.ChatHub.Models
{
  [BsonCollection("Messages")]
  public class Message : BaseEntity
  {
    public string Text { get; set; }
    public string IsImage { get; set; }
    public string ImageUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public User CreatedBy { get; set; }
    public DateTime? UpdateDate { get; set; }
    public bool IsDeleted { get; set; }
  }
}
