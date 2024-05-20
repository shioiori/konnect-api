namespace UTCClassSupport.Chat.Models
{
  public class Chat
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedDate { get; set; }
    public User CreatedBy { get; set; }

    public string Avatar { get; set; } = "https://i.imgur.com/su4KGCT.png";
    public ICollection<User> Users { get; set; }
    public ICollection<Message> Messages { get; set; }
  }
}
