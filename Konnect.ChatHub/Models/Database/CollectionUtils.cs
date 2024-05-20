using Konnect.ChatHub.Attributes;

namespace Konnect.ChatHub.Models.Database
{
  public class CollectionUtils<T>
  {
    public static string GetCollectionName()
    {
      return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true).FirstOrDefault()
          as BsonCollectionAttribute).CollectionName;
    }
  }
}
