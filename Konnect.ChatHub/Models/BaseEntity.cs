using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Konnect.ChatHub.Models
{
  public class BaseEntity
  {
    [BsonId]
    public Guid Id { get; set; }
  }
}
