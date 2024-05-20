namespace UTCClassSupport.Chat.Models
{
  public class Group
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
  }
}
