namespace UTCClassSupport.Chat.Models
{
  public class User
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string Avatar { get; set; }
  }
}
