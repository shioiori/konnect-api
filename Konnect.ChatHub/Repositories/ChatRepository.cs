using Konnect.ChatHub.Models;
using Konnect.ChatHub.Models.Database;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Konnect.ChatHub.Repositories
{
  public class ChatRepository : BaseRepository<Chat>, IChatRepository
  {
    public ChatRepository(
        IOptions<ChatDatabaseSettings> ChatDatabaseSettings) : base(ChatDatabaseSettings)
    {
    }

    public Task<List<Chat>> GetChats(string groupId, string userId)
    {
      return Task.FromResult(_TCollection.Find(x => x.Group.Id == groupId 
        && x.Users.Select(x => x.Id).Contains(userId)).ToList());
    }
  }

  public interface IChatRepository : IBaseRepository<Chat>
  {
    Task<List<Chat>> GetChats(string groupId, string userId);
  }

}
